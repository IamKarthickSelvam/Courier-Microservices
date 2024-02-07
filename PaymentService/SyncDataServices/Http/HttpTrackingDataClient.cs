using System.Text.Json;
using System.Text;
using PaymentService.DTOs;

namespace PaymentService.SyncDataServices.Http
{
    public class HttpTrackingDataClient : IHttpTrackingDataClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public HttpTrackingDataClient(
            HttpClient httpClient,
            IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task SendPaymentToTracking(PaymentReadDto payment)
        {
            var httpContent = new StringContent(
                JsonSerializer.Serialize(payment),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PostAsync($"{_configuration["TrackingService"]}", httpContent);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("--> Sync POST to TrackingService was OK!");
            }
            else
            {
                Console.WriteLine("--> Sync POST to TrackingService was NOT OK!");
            }
        }
    }
}
