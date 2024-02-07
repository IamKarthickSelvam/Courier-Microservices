using AutoMapper;
using BookingService.DTOs;
using Microsoft.OpenApi.Writers;
using PaymentService.DTOs;
using System.Text.Json;
using TrackingService.Data;
using TrackingService.Models;

namespace TrackingService.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMapper _mapper;
        private readonly ITrackingRepository _repository;

        public EventProcessor(IServiceScopeFactory scopeFactory, IMapper mapper, ITrackingRepository repository)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
            _repository = repository;
        }

        public void ProcessEvent(string message)
        {
            var eventType = DetermineEvent(message);

            switch (eventType)
            {
                case EventType.BookingPublished:
                case EventType.PaymentPublished:
                    UpdateCourier(message, eventType);
                    break;
                default:
                    break;
            }
        }

        private EventType DetermineEvent(string notification)
        {
            Console.WriteLine("--> Determining Event");

            var eventType = JsonSerializer.Deserialize<GenericEventDto>(notification);

            switch (eventType.Event) 
            {
                case "Booking_Published":
                    Console.WriteLine("--> Booking Published Event Detected");
                    return EventType.BookingPublished;
                case "Payment_Published":
                    Console.WriteLine("--> Payment Published Event Detected");
                    return EventType.PaymentPublished;
                default:
                    Console.WriteLine("--> Could not determine the event type");
                    return EventType.Undetermined;
            }
        }

        private async void UpdateCourier(string publishedMessage, EventType eventType)
        {
            using var scope = _scopeFactory.CreateScope();

            var repo = scope.ServiceProvider.GetRequiredService<ITrackingRepository>();

            try
            {
                Courier courier = new();
                switch (eventType)
                {
                    case EventType.BookingPublished:

                        var bookingPublishedDto = JsonSerializer.Deserialize<BookingPublishedDto>(publishedMessage);

                        courier = _mapper.Map<Courier>(bookingPublishedDto);

                        // Payment still pending so set Status to 'P' and PaymentTime null
                        courier.PaymentStatus = "P";
                        courier.PaymentTime = null;

                        break;

                    case EventType.PaymentPublished:

                        var paymentPublishedDto = JsonSerializer.Deserialize<PaymentPublishedDto>(publishedMessage);

                        courier = _mapper.Map<Courier>(paymentPublishedDto);

                        break;

                    default:

                        Console.WriteLine("--> Could not identify the type of inbound message!");

                        break;
                }

                var existingCourier = await _repository.GetByBookingIdAsync(courier.BookingId);

                if (existingCourier != null)
                {
                    await _repository.UpdateAsync(existingCourier.Id, courier);
                    await Console.Out.WriteLineAsync($"--> Updated Courier id: {existingCourier.Id} and Booking id: {existingCourier.BookingId}");
                } 
                else
                {
                    await _repository.CreateAsync(courier);
                    await Console.Out.WriteLineAsync($"--> Created new courier for Booking id: {courier.BookingId}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not add inbound event object to DB {ex.Message}");
            }
        }
    }

    enum EventType
    {
        BookingPublished,
        PaymentPublished,
        Undetermined
    }
}
