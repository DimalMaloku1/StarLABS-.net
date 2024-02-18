using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;

namespace Domain.Contracts
{
    public interface IBillRepository
    {
        Task<IEnumerable<Bill>> GetAllBills();
        Task<Bill> GetBillById(Guid id);
        Task<Bill> AddBill(Bill bill);
        Task<Bill> UpdateBill(Bill bill);
        Task<Bill> DeleteBill(Bill bill);
        Task<IEnumerable<Bill>> GetBillsByBookingId(Guid bookingId);
        Task<IEnumerable<Bill>> GetBillsByUser(Guid userId);

    }
}
