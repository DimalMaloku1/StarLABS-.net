using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
    internal sealed class BillRepository : IBillRepository { 
        private readonly DataContext _context;
       public BillRepository(DataContext context)
           {
            _context = context;
        }
    
        public async Task<Bill> AddBill(Bill bill)
        {
             _context.Bills.Add(bill);
             await _context.SaveChangesAsync();
            return bill;
        }

        public async Task<Bill> DeleteBill(Bill bill)
        {
            _context.Bills.Remove(bill);
            await _context.SaveChangesAsync();
            return bill;
        }

        public async Task<IEnumerable<Bill>> GetAllBills()
        {
            var bills = await _context.Bills.ToListAsync();
            return bills;


        }

        public async Task<Bill> GetBillById(Guid id)
        {
            var bill = await _context.Bills.FindAsync(id);
            return bill;
        }

        public async Task<IEnumerable<Bill>> GetBillsByBookingId(Guid bookingId)
        {
            var bills = await _context.Bills.Where(b => b.BookingId == bookingId).ToListAsync();  
            return bills;
        }

        public async Task<IEnumerable<Bill>> GetBillsByUser(Guid userId)
        {
            var bills = await _context.Bills.Where(b => b.Booking.UserId == userId).ToListAsync();
            return bills;
        }

        public async Task<Bill> UpdateBill(Bill bill)
        {
           _context.Bills.Update(bill);
            await _context.SaveChangesAsync();
            return bill;
        }
    }
}
