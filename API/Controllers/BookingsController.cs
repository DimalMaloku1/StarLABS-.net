using Application.DTOs;
using Application.Services.BookingServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingsController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet]
        public async Task<IEnumerable<BookingDto>> GetAllBookings()
        {
            var bookings=  await _bookingService.GetAllBookingsAsync();
            return bookings;
        }


    }
}
