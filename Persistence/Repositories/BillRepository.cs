using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts;
using Domain.Models;

namespace Persistence.Repositories
{
    public class BillRepository : IBillRepository
    {
        public Task<Bill> AddBill(Bill bill)
        {
            throw new NotImplementedException();
        }

        public Task<Bill> DeleteBill(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Bill>> GetAllBills()
        {
            throw new NotImplementedException();
        }

        public Task<Bill> GetBill(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Bill>> GetBillsByBookingId(int bookingId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Bill>> GetBillsByUser(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<Bill> UpdateBill(Bill bill)
        {
            throw new NotImplementedException();
        }
    }
}
