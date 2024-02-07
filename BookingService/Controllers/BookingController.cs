using AutoMapper;
using BookingService.AsyncDataServices;
using BookingService.Data;
using BookingService.DTOs;
using BookingService.Models;
using BookingService.SyncDataServices.Grpc;
using BookingService.SyncDataServices.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookingService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly IBookingRepository _repository;
        private readonly IMapper _mapper;
        private readonly IHttpPaymentDataClient _httpPaymentDataClient;
        private readonly IMessageBusClient _messageBusClient;
        private readonly IPaymentDataClient _paymentDataClient;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BookingController(
            IBookingRepository repository,
            IMapper mapper,
            IHttpPaymentDataClient httpPaymentDataClient,
            IMessageBusClient messageBusClient,
            IPaymentDataClient paymentDataClient,
            IWebHostEnvironment webHostEnvironment)
        {
            _repository = repository;
            _mapper = mapper;
            _httpPaymentDataClient = httpPaymentDataClient;
            _messageBusClient = messageBusClient;
            _paymentDataClient = paymentDataClient;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet("Pincode")]
        public ActionResult<IEnumerable<PincodeReadDto>> GetPincode()
        {
            try
            {
                Console.WriteLine("--> Getting all Pincodes...");

                var pincodeList = _repository.GetAllPincodes();

                return Ok(_mapper.Map<IEnumerable<PincodeReadDto>>(pincodeList));
            }
            catch (Exception)
            {
                return StatusCode(500, "Unable to retrieve all available Pincodes, please refresh the page and try again.");
            }
        }

        [HttpGet("{id}", Name = "GetBookingById")]
        public ActionResult<BookingReadDto> GetBookingById(int id) 
        {
            var bookingItem = _repository.GetBookingById(id);
            if (bookingItem != null)
            {
                return Ok(_mapper.Map<BookingReadDto>(bookingItem));
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<BookingReadDto>>> CreateBookingAsync(BookingCreateDto bookingCreateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                bookingCreateDto.BookedTime = DateTime.Now;
                var bookingModel = _mapper.Map<Booking>(bookingCreateDto);
                _repository.CreateBooking(bookingModel);
                _repository.SaveChanges();

                var bookingReadDto = _mapper.Map<BookingReadDto>(bookingModel);

                //Send Sync HTTP Message
                //DEV only
                if (_webHostEnvironment.IsDevelopment())
                {
                    try
                    {
                        await _httpPaymentDataClient.SendBookingToPayment(bookingReadDto.Id);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"--> Could not send synchronously: {ex.Message}");
                    }
                }

                //Send Sync remote call
                try
                {
                    var request = new GrpcBookingModel()
                    {
                        BookingId = bookingModel.Id,
                    };

                    _paymentDataClient.InitiatePayment(request);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not send payment: {ex.Message}");
                }

                //Send Async Message
                try
                {
                    var bookingPublishedDto = _mapper.Map<BookingPublishedDto>(bookingReadDto);
                    bookingPublishedDto.Event = "Booking_Published";
                    _messageBusClient.PublishNewBooking(bookingPublishedDto);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not send asynchronously: {ex.Message}");
                }

                return CreatedAtRoute(nameof(GetBookingById), new { bookingReadDto.Id }, bookingReadDto);
            }
            catch (Exception ex)
            {
                var errorMessage = $"Sorry, we are unable to process your Booking at the moment, please try again after some time. Error message: {ex.Message}";
                await Console.Out.WriteLineAsync($"-->{errorMessage}");
                return StatusCode(500, errorMessage);
            }
        }

        [HttpGet]
        public ActionResult<IEnumerable<Booking>> GetAllBookings()
        {
            try
            {
                Console.WriteLine("--> Getting all Bookings...");

                var bookingList = _repository.GetAllBookings();

                return Ok(_mapper.Map<IEnumerable<BookingReadDto>>(bookingList));
            }
            catch (Exception)
            {
                return StatusCode(500, "Unable to retrieve all Bookings, please refresh the page and try again.");
            }
        }
    }
}
