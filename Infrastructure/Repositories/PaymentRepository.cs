using Domain.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly DataContext _db;

        public PaymentRepository(DataContext db)
        {
            _db = db;
        }

        public async Task<Payment> GetByIdAsync(Guid id)
        {
            var payment = await _db.Payments.FindAsync(id);
            return payment;
        }


        public async Task<IEnumerable<Payment>> GetAllAsync()
        {
            var payments = await _db.Payments.ToListAsync();
            return payments;
        }


        public async Task AddAsync(Payment payment)
        {
            await _db.Payments.AddAsync(payment);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Guid id, Payment payment)
        {
            var existingPayment = await _db.Payments.FindAsync(id);
            if (existingPayment != null)
            {
                existingPayment.PaymentMethod = payment.PaymentMethod;
                existingPayment.BillId = payment.BillId;
                existingPayment.TotalAmount = payment.TotalAmount;

                await _db.SaveChangesAsync();
            }
        }


        public async Task DeleteAsync(Guid id)
        {
            var payment = await GetByIdAsync(id);
            if (payment != null)
            {
                _db.Payments.Remove(payment);
                await _db.SaveChangesAsync();
            }
        }
    }
}
