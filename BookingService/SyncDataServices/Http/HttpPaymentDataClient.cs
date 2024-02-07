using System.Text.Json;
using System.Text;

namespace BookingService.SyncDataServices.Http
{
    public class HttpPaymentDataClient : IHttpPaymentDataClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public HttpPaymentDataClient(
            HttpClient httpClient,
            IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task SendBookingToPayment(int id)
        {

            var httpContent = new StringContent(
                JsonSerializer.Serialize(id),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PostAsync($"{_configuration["PaymentService"]}/Stripe", httpContent);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("--> Sync POST to PaymentService was OK!");
            }
            else
            {
                Console.WriteLine("--> Sync POST to PaymentService was NOT OK!");
            }
        }
    }
}
