using Application.DTOs;
using AutoMapper;
using Domain.Contracts;
using Domain.Models;


namespace Application.Services.BillService
{
    internal class BillService : IBillService
    {
        private readonly IBillRepository _billRepository;
        private readonly IMapper _mapper;
        private readonly IBookingRepository _bookingRepository;

        public BillService(IBillRepository billRepository, IMapper mapper, IBookingRepository bookingRepository)
        {
            _billRepository = billRepository;
            _mapper = mapper;
            _bookingRepository = bookingRepository;
        }

        public async Task<BillDto> AddBill(BillDto billDto)
        {
            decimal totalPrice = 0;
            var booking = await _bookingRepository.GetBookingByIdAsync(billDto.BookingId);


            try
            {
                if (booking == null && (billDto == null || booking?.Room?.RoomType == null))
                {
                    throw new InvalidOperationException("Booking or Room information is missing.");
                }

                billDto.DaysSpent = (booking.CheckOutDate - booking.CheckInDate).Days;
                totalPrice = (decimal)(billDto.DaysSpent * booking.Room.RoomType.Price);

                billDto.TotalAmount = totalPrice;

                var bill = _mapper.Map<Bill>(billDto);

                await _billRepository.AddBill(bill);
                return billDto;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }
            finally
            {
                Console.WriteLine($"Total amount of bill: {totalPrice}");
            }
        }

        public async Task DeleteBill(Guid id)
        {
            var bill = await _billRepository.GetBillById(id);
            if (bill == null) throw new InvalidOperationException("Could not find bill");
            try
            {
                await _billRepository.DeleteBill(bill);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IEnumerable<BillDto>> GetAllBills()
        {
            var bills = await _billRepository.GetAllBills();
            var billDtos = _mapper.Map<IEnumerable<BillDto>>(bills);
            foreach (var bill in billDtos)
            {
                bill.Username = bill.Booking.User.UserName;
                bill.UserId = bill.Booking.UserId;
            }

            return billDtos;
        }


        public async Task<BillDto> GetBillById(Guid id)
        {
            if (id == Guid.Empty) return null;

            var bill = await _billRepository.GetBillById(id);
            decimal totalAmount = (decimal)((bill.Booking.CheckOutDate - bill.Booking.CheckInDate).Days *
                                            bill.Booking.Room.RoomType.Price);
            bill.TotalAmount = (double)totalAmount;
            await _billRepository.UpdateBill(bill);
            var billDto = _mapper.Map<BillDto>(bill);
            billDto.Username = bill.Booking.User.UserName;
            billDto.DaysSpent = (bill.Booking.CheckOutDate - bill.Booking.CheckInDate).Days;
            billDto.TotalAmount = totalAmount;
            billDto.RoomPrice = bill.Booking.Room.RoomType.Price;
            billDto.RoomType = bill.Booking.Room.RoomType.Type;

            return billDto;
        }

        public async Task<IEnumerable<BillDto>> GetBillsByBookingId(Guid bookingId)
        {
            if (bookingId != Guid.Empty)
            {
                var bills = await _billRepository.GetBillsByBookingId(bookingId);
                return _mapper.Map<IEnumerable<BillDto>>(bills);
            }

            return null;
        }

        public async Task<IEnumerable<BillDto>> GetBillsByUser(Guid userId)
        {
            if (userId != Guid.Empty)
            {
                var bills = await _billRepository.GetBillsByUser(userId);
                return _mapper.Map<IEnumerable<BillDto>>(bills);
            }

            return null;
        }

        public async Task<BillDto> UpdateBill(BillDto bill)
        {
            if (bill != null)
            {
                var billToUpdate = _mapper.Map<Bill>(bill);
                await _billRepository.UpdateBill(billToUpdate);
            }

            return bill;
        }
    }
}