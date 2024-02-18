using Application.DTOs;
using Application.Responses;
using AutoMapper;
using Domain.Contracts;
using Domain.Models;



namespace Application.Services.BillService
{
    internal class BillService : IBillService { 
        private readonly IBillRepository _billRepository;
        private readonly IMapper _mapper;
        public BillService(IBillRepository billRepository, IMapper mapper)
        {
            _billRepository = billRepository;
            _mapper = mapper;
        }

    
        public async Task<ApiResponse> AddBill(BillDto billDto)
        {
            if(billDto != null)
            {
                var bill = _mapper.Map<Bill>(billDto);
                await _billRepository.AddBill(bill); ;
                return new ApiResponse(200, "Bill added successfully");
            }
            return new ApiResponse(400, "Bill not added");
           
            

        }

        public async Task<ApiResponse> DeleteBill(Guid id)
        {
            if(id != Guid.Empty)
            {
                var bill = await _billRepository.GetBillById(id);
                await _billRepository.DeleteBill(bill);
                return new ApiResponse(200,"Bill deleted succesfully");
            }
            return new ApiResponse(400, "Bill not deleted");
        }

        public async Task<IEnumerable<BillDto>> GetAllBills()
        {
            var bills = await _billRepository.GetAllBills();
            return  _mapper.Map<IEnumerable<BillDto>>(bills);
           
        }

        public async Task<BillDto> GetBillById(Guid id)
        {
            if(id != Guid.Empty)
            {
                var bill = await _billRepository.GetBillById(id);
                return _mapper.Map<BillDto>(bill);
            }
            return null;
        }

        public async Task<IEnumerable<BillDto>> GetBillsByBookingId(Guid bookingId)
        {
            if(bookingId != Guid.Empty)
            {
                var bills = await _billRepository.GetBillsByBookingId(bookingId);
                return _mapper.Map<IEnumerable<BillDto>>(bills);
                
            }
            return null;
        }

        public async Task<IEnumerable<BillDto>> GetBillsByUser(Guid userId)
        {
            if(userId != null)
            {
                var bills = await _billRepository.GetBillsByUser(userId);
                return _mapper.Map<IEnumerable<BillDto>>(bills);
            }
            return null;
        }


        public async Task<ApiResponse> UpdateBill(BillDto bill)
        {
            if(bill != null)
            {
                var billToUpdate = _mapper.Map<Bill>(bill);
                await _billRepository.UpdateBill(billToUpdate);
               return new ApiResponse(200, "Bill succesfully updated");
            }
            return new ApiResponse(400, "Bill not updated");
        }
    }
}
