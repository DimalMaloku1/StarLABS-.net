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
        public async Task<BookingDto> CreateAsync(BookingDto bookingDto, Guid userId, string baseUrl)
        {
            try
            {
                var booking = _mapper.Map<Booking>(bookingDto);
                booking.UserId = userId;
                await _bookingRepository.Add(booking);
                var billDto = new BillDto
                {
                    BookingId = booking.Id,
                    UserId = booking.UserId
                };
                var createdBill = await _billService.AddBill(billDto);
                var bills = await _billService.GetBillsByBookingId(booking.Id);

                if (bills == null || !bills.Any())
                {
                    throw new Exception("No bills found for the created booking.");
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

                    // Send confirmation email
                    var html = await _renderer.RenderPartialToStringAsync("_BookingConfirmation", bookingConfirmationDto);
                    await _emailService.SendBookingConfirmationEmailAsync(user.Email, html);

                    return _mapper.Map<BookingDto>(booking);
                }
                else
                {
                    throw new Exception("No bills found for the created booking.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while creating booking and billing: {ex.Message}");
                throw;
            }
        }

        private string GenerateBillDetailsUrl(Guid billId, string baseUrl)
        {
            string url = $"{baseUrl}/Bill/Details/{billId}";
            return url;
        }


        public async Task UpdateAsync(Guid bookingId, BookingDto updatedBookingDto)
        {
            Booking booking = await _bookingRepository.GetBookingByIdAsync(bookingId);

            try
            {
                _ = _mapper.Map(updatedBookingDto, booking);
                await _bookingRepository.Update(booking);

                int daysSpent = (updatedBookingDto.CheckOutDate - updatedBookingDto.CheckInDate).Days;
                if (booking.Room != null && booking.Room.RoomType != null)
                {
                    IEnumerable<BillDto> bills = await _billService.GetBillsByBookingId(bookingId);
                    foreach (BillDto bill in bills)
                    {
                        BillDto billDto = _mapper.Map<BillDto>(bill);
                        billDto.DaysSpent = daysSpent;
                        billDto.TotalAmount = (decimal)(daysSpent * booking.Room.RoomType.Price);
                        _ = await _billService.UpdateBill(billDto);
                    }
                }
                else
                {
                }
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


    }
}