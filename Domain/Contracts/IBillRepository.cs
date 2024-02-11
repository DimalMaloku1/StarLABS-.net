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
        Task<Bill> GetBill(int id);
        Task<Bill> AddBill(Bill bill);
        Task<Bill> UpdateBill(Bill bill);
        Task<Bill> DeleteBill(int id);
        Task<IEnumerable<Bill>> GetBillsByBookingId(int bookingId);
        Task<IEnumerable<Bill>> GetBillsByUser(string userId);

    }
}
