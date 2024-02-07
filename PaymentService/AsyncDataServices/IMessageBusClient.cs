using PaymentService.DTOs;

namespace PaymentService.AsyncDataServices
{
    public interface IMessageBusClient
    {
        void PublishNewPayment(PaymentPublishedDto paymentPublishedDto);
    }
}
