using Domain.Models;

namespace Domain.Contracts
{
    public interface IPaymentRepository
    {
        Task<Payment> GetByIdAsync(Guid id);
        Task<IEnumerable<Payment>> GetAllAsync();
        Task AddAsync(Payment payment);
        Task UpdateAsync(Guid id, Payment payment);
        Task DeleteAsync(Guid id);
    }
}
