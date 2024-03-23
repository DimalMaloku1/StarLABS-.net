using Application.Core;
using Application.DTOs;
using Application.Services.BillService;
using Application.Services.EmailServices;
using Application.Services.RazorServices;
using Application.Services.RoomServices;
using Application.Services.RoomTypeServices;
using AutoMapper;
using Domain.Contracts;
using Domain.Models;
using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;

namespace Application.Services.BookingServices
{
    internal sealed class BookingService(IRoomServices _roomService, IRoomTypeServices _roomTypeService,
        IBookingRepository _bookingRepository, IMapper _mapper, IBillService _billService,
        UserManager<AppUser> _userManager, IEmailService _emailService, IRazorPartialToStringRenderer _renderer) : IBookingService
    {
        public async Task<IEnumerable<BookingDto>> GetAllBookingsAsync()
        {
            IEnumerable<Booking> bookings = await _bookingRepository.GetBookingsAsync();
            IEnumerable<BookingDto> bookingsDto = _mapper.Map<IEnumerable<BookingDto>>(bookings);
            return bookingsDto;
        }

        public async Task<BookingDto> GetBookingByIdAsync(Guid id)
        {
            Booking booking = await GetBookingById(id);
            BookingDto bookingDto = _mapper.Map<BookingDto>(booking);
            return bookingDto;
        }
       

        public async Task<Result<BookingDto>> CreateAsync(BookingDto bookingDto, Guid userId, string baseUrl)
        {
            try
            {
                var availableRoom = await AvailableRoom(bookingDto);

                if (availableRoom != null)
                {
                    bookingDto.UserId = userId;
                    bookingDto.TotalPrice = await CalculateTotalPrice(bookingDto.RoomTypeId, bookingDto);
                    var booking = _mapper.Map<Booking>(bookingDto);

                    booking.RoomId = availableRoom.Id;

                    await _bookingRepository.Add(booking);

                    var billDto = new BillDto
                    {
                        BookingId = booking.Id,
                        UserId = userId
                    };
                    var createdBill = await _billService.AddBill(billDto);
                    var bills = await _billService.GetBillsByBookingId(booking.Id);

                    if (bills == null || !bills.Any())
                    {
                        return Result<BookingDto>.Failure("No bills found for the created booking.");
                    }

                    var firstBill = bills.FirstOrDefault();
                    if (firstBill != null)
                    {
                        var billDetailsUrl = GenerateBillDetailsUrl(firstBill.Id, baseUrl);
                        var user = await _userManager.FindByIdAsync(userId.ToString());
                        int daysSpent = (booking.CheckOutDate - booking.CheckInDate).Days;
                        var roomPrice = booking.Room.RoomType.Price;
                        var totalPrice = (decimal)(daysSpent * roomPrice);
                        var bookingConfirmationDto = new BookingConfirmationDto
                        {
                            BookingId = booking.Id,
                            UserId = booking.UserId,
                            BillId = firstBill.Id,
                            Username = booking.User.UserName,
                            CheckInDate = booking.CheckInDate,
                            CheckOutDate = booking.CheckOutDate,
                            RoomType = booking.Room.RoomType.Type,
                            RoomNumber = booking.Room.RoomNumber,
                            DaysSpent = daysSpent,
                            TotalAmount = (double)totalPrice,
                            BillDetailsUrl = billDetailsUrl
                        };

                        var html = await _renderer.RenderPartialToStringAsync("_BookingConfirmation", bookingConfirmationDto);
                        await _emailService.SendBookingConfirmationEmailAsync(user.Email, html);

                        var createdBooking = _mapper.Map<BookingDto>(booking);
                        return Result<BookingDto>.Success(createdBooking);
                    }

                    return Result<BookingDto>.Failure("No bills found for the created booking.");
                }
                return Result<BookingDto>.Failure("No available rooms for the selected dates!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while creating booking and billing: {ex.Message}");
                throw;
            }
        }
        public async Task<Result<BookingDto>> UpdateAsync(Guid bookingId, BookingDto updatedBookingDto)
        {
            Booking booking = await _bookingRepository.GetBookingByIdAsync(bookingId);

            try
            {
                var availableRoom = await AvailableRoom(updatedBookingDto);
                if (availableRoom != null)
                {
                    updatedBookingDto.RoomId = availableRoom.Id;
                    updatedBookingDto.TotalPrice = await CalculateTotalPrice(updatedBookingDto.RoomTypeId, updatedBookingDto);
                    _mapper.Map(updatedBookingDto, booking);

                    // Update the booking in the repository
                    await _bookingRepository.Update(booking);

                    return Result<BookingDto>.Success(updatedBookingDto);
                }
                return Result<BookingDto>.Failure("No available rooms for the given dates.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while updating booking and bills: {ex.Message}");
                throw;
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            Booking booking = await GetBookingById(id);
            await _bookingRepository.Delete(booking);

        }

        public async Task<NewBookingDropDownsDTO> GetNewBookingDropDownsValues()
        {
            NewBookingDropDownsDTO response = new()
            {
                Rooms = await _roomService.GetAllRoomsAsync(),
                RoomTypes = await _roomTypeService.GetAllRoomTypesAsync()
            };

            return response;

        }

        private async Task<Booking> GetBookingById(Guid id)
        {
            return await _bookingRepository.GetBookingByIdAsync(id);
        }


        private async Task<double> CalculateTotalPrice(Guid roomtypeId, BookingDto bookingDto)
        {
            var roomtype = await _roomTypeService.GetRoomTypeByIdAsync(roomtypeId);
            if (roomtype == null) return 0;
            int days = (bookingDto.CheckOutDate - bookingDto.CheckInDate).Days;

            return (double)(roomtype.Price * days);
        }

        private async Task<RoomDto> AvailableRoom(BookingDto bookingDto)
        {
            var roomTypeId = bookingDto.RoomTypeId;
            var checkInDate = bookingDto.CheckInDate;
            var checkOutDate = bookingDto.CheckOutDate;

            var availableRoom = await _roomService.GetAvailableRoomAsync(roomTypeId, checkInDate, checkOutDate);
            return availableRoom;
        }

        private string GenerateBillDetailsUrl(Guid billId, string baseUrl)
        {
            string url = $"{baseUrl}/Bill/Details/{billId}";
            return url;
        }

    }
}