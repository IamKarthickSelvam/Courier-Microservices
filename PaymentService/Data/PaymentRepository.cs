using Microsoft.EntityFrameworkCore;
using PaymentService.Models;

namespace PaymentService.Data
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly AppDbContext _context;

        public PaymentRepository(AppDbContext context)
        {
            _context = context;
        }

        public void CreatePayment(Payment payment)
        {
            _context.Payments.Add(payment);
        }

        public void UpdatePayment(Payment payment)
        {
            var paymentToBeUpdated = _context.Payments.FirstOrDefault(x => x.Id == payment.Id);
            _context.Payments.Update(paymentToBeUpdated);
        }

        public IEnumerable<Payment> GetAllPayments()
        {
            return _context.Payments.ToList();
        }

        public Payment GetPaymentByBookingId(int bookingid)
        {
            return _context.Payments.FirstOrDefault(x => x.BookingId == bookingid);
        }

        public Payment GetPaymentById(int id)
        {
            return _context.Payments.FirstOrDefault(x => x.Id == id);
        }

        public bool SaveChanges()
        {
            return _context.SaveChanges() >= 0;
        }
    }
}
