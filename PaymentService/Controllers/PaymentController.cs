using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PaymentService.AsyncDataServices;
using PaymentService.Data;
using PaymentService.DTOs;
using PaymentService.Models;
using PaymentService.SyncDataServices.Http;
using Stripe;
using Stripe.Checkout;

namespace PaymentService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly StripeSettings _stripeSettings;
        private readonly IPaymentRepository _repository;
        private readonly IHttpTrackingDataClient _httpTrackingDataClient;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMessageBusClient _messageBusClient;

        public PaymentController(
            IOptions<StripeSettings> stripeSettings,
            IPaymentRepository repository,
            IHttpTrackingDataClient httpTrackingDataClient,
            IMapper mapper,
            IConfiguration configuration,
            IWebHostEnvironment webHostEnvironment,
            IMessageBusClient messageBusClient)
        {
            _stripeSettings = stripeSettings.Value;
            _repository = repository;
            _httpTrackingDataClient = httpTrackingDataClient;
            _mapper = mapper;
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
            _messageBusClient = messageBusClient;
        }

        [HttpPost]
        public ActionResult<StripeSessionDto> CreateCheckoutSession([FromBody] BookingReadDto bookingModel) 
        {
            try
            {
                var currency = "usd";
                var successUrl = _configuration.GetValue<string>("StripeUrls:Success");
                var cancelUrl = _configuration.GetValue<string>("StripeUrls:Failed");
                StripeConfiguration.ApiKey = _stripeSettings.SecretKey;

                var options = new SessionCreateOptions
                {
                    PaymentMethodTypes = new List<string>
                {
                    "card"
                },
                    LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Currency = currency,
                            UnitAmount = Convert.ToInt32(bookingModel.Amount) * 100,
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = "Product Name",
                                Description = "Product Description"
                            }
                        },
                        Quantity = 1
                    },
                },
                    Mode = "payment",
                    SuccessUrl = successUrl,
                    CancelUrl = cancelUrl
                };

                var service = new SessionService();
                var session = service.Create(options);

                return new StripeSessionDto()
                {
                    Id = session.Id,
                    Url = session.Url,
                    BookingId = bookingModel.Id
                };
            }
            catch (Exception ex)
            {
                var errorMessage = $"--> Error while creating Stripe session: {ex.Message}";
                Console.WriteLine(errorMessage);
                return BadRequest(errorMessage);
            }
        }

        [HttpPost("ConfirmPayment")]
        public async Task<ActionResult> ConfirmPayment(StripeSessionDto stripeSessionDto)
        {
            try
            {
                var service = new SessionService();
                var session = service.Get(stripeSessionDto.Id);
                var bookingId = _repository.GetPaymentByBookingId(stripeSessionDto.BookingId);

                var returnStatusCode = 0;
                var returnStatusMessage = "";
                Payment newPayment = new();

                switch (session.PaymentStatus)
                {
                    case "paid":
                        newPayment.Status = "Done";
                        newPayment.StatusShort = "D";
                        
                        returnStatusCode = 200; //OK
                        returnStatusMessage = "Payment Done";
                        break;
                    default:
                        newPayment.Status = "Failed";
                        newPayment.StatusShort = "F";

                        returnStatusCode = 500; //Internal Server Error
                        returnStatusMessage = "Payment Failed, Please retry";
                        break;
                }
                newPayment.PaymentTime = DateTime.Now;
                newPayment.BookingId = stripeSessionDto.BookingId;

                _repository.CreatePayment(newPayment);
                _repository.SaveChanges();

                var paymentReadDto = _mapper.Map<PaymentReadDto>(newPayment);

                //Send Sync HTTP Message
                //DEV only
                if (_webHostEnvironment.IsDevelopment())
                {
                    try
                    {
                        await _httpTrackingDataClient.SendPaymentToTracking(paymentReadDto);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"--> Could not send synchronously: {ex.Message}");
                    }
                }

                //Send Async Message
                try
                {
                    var paymentPublishedDto = _mapper.Map<PaymentPublishedDto>(paymentReadDto);
                    paymentPublishedDto.Event = "Payment_Published";
                    _messageBusClient.PublishNewPayment(paymentPublishedDto);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not send asynchronously: {ex.Message}");
                }

                return StatusCode(returnStatusCode, returnStatusMessage);
            }
            catch (Exception ex)
            {
                var errorMessage = $"--> Error while updating Payment: {ex.Message}";
                Console.WriteLine(errorMessage);
                return StatusCode(500, errorMessage);
            }
        }

        [HttpPost("{id}", Name = "GetPaymentBydId")]
        public ActionResult<PaymentReadDto> GetPaymentById(int id)
        {
            var paymentItem = _repository.GetPaymentById(id);
            if (paymentItem == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<PaymentReadDto>(paymentItem));
        }
    }
}
