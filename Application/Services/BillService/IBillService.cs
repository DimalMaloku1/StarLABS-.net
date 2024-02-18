using Application.DTOs;
using Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.BillService
{
    public interface IBillService
    {
        Task<IEnumerable<BillDto>> GetAllBills();
        Task<BillDto> GetBillById(Guid id);
        Task<ApiResponse> AddBill(BillDto bill);
        Task<ApiResponse> UpdateBill(BillDto bill);
        Task<ApiResponse> DeleteBill(Guid Id);
        Task<IEnumerable<BillDto>> GetBillsByBookingId(Guid bookingId);
        Task<IEnumerable<BillDto>> GetBillsByUser(Guid userId);
    }
}
