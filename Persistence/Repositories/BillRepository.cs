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
             await _context.Bills.AddAsync(bill);
             await _context.SaveChangesAsync();
            return bill;
        }

        public async Task DeleteBill(Bill bill)
        {
             _context.Bills.Remove(bill);
            await _context.SaveChangesAsync();
            
        }

        public async Task<IEnumerable<Bill>> GetAllBills()
        {
            var bills = await _context.Bills
                .Include(b => b.Booking)
                .ThenInclude(bo => bo.User)
                .OrderBy(b=>b.CreatedAt)
                .ToListAsync();
            return bills;


        }

        public async Task<Bill> GetBillById(Guid id)
        {
            var bill = await _context.Bills
                .Include(b => b.Booking)
                .ThenInclude(b=>b.Room)
                .ThenInclude(b=>b.RoomType)
                .Include(b=>b.Booking)
                .ThenInclude(b=>b.User)
                .FirstOrDefaultAsync(b => b.Id == id);
            
            
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
