using Application.DTOs;
using Application.Services.BookingServices;
using Application.Services.RoomTypeServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace API.Controllers
{
    public class BookingsController : Controller
    {
        private readonly IBookingService _bookingService;
        private readonly IRoomTypeServices _roomTypeService;

        public BookingsController(IBookingService bookingService, IRoomTypeServices roomTypeService)
        {
            _bookingService = bookingService;
            _roomTypeService = roomTypeService;
        }

        public async Task<IActionResult> Index()
        {
            var bookings = await _bookingService.GetAllBookingsAsync();
            return View("Index", bookings);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var booking = await _bookingService.GetBookingByIdAsync(id);
            return View("Details", booking);
        }
        public async Task<IActionResult> Create()
        {
            var bookingDropDownData = await _bookingService.GetNewBookingDropDownsValues();

            ViewBag.RoomTypes = new SelectList(bookingDropDownData.RoomTypes, "Id", "Type");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookingDto bookingDto)
        {
            var bookingDropDownData = await _bookingService.GetNewBookingDropDownsValues();
            ViewBag.RoomTypes = new SelectList(bookingDropDownData.RoomTypes, "Id", "Type");
            if (!ModelState.IsValid)
            {
                return View(bookingDto);
            }
            await _bookingService.CreateAsync(bookingDto);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var booking = await _bookingService.GetBookingByIdAsync(id);
            return View("Edit", booking);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, BookingDto bookingDto)
        {
            await _bookingService.UpdateAsync(id, bookingDto);
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Delete(Guid id)
        {
            var booking = await _bookingService.GetBookingByIdAsync(id);
            return View("Delete", booking);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _bookingService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

    }
}
