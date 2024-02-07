using PaymentService.Models;

namespace PaymentService.Data
{
    public interface IPaymentRepository
    {
        bool SaveChanges();
        IEnumerable<Payment> GetAllPayments();
        Payment GetPaymentByBookingId(int bookingid);
        Payment GetPaymentById(int id);
        void CreatePayment(Payment payment);
    }
}
