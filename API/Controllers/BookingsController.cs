using System.Security.Claims;
using API.Views.Dashboard;
using Application.DTOs;
using Application.DTOs.AccountDTOs;
using Application.Services.BillService;
using Application.Services.BookingServices;
using Application.Services.RoomServices;
using Application.Services.RoomTypeServices;
using Domain.Models;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace API.Controllers
{
    [Authorize]
    public class BookingsController(UserManager<AppUser> _userManager, IBookingService _bookingService, IValidator<BookingDto> _bookingValidator, IBillService _billService, IRoomTypeServices _roomTypeService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var bookings = await _bookingService.GetAllBookingsAsync();
            return View("Index", bookings);
        }
        public async Task<IActionResult> Details(Guid id)
        {
            try
            {
                var booking = await _bookingService.GetBookingByIdAsync(id);
                if (booking is null) return NotFound();
                return View("Details", booking);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", ex.Message);
                return RedirectToAction(nameof(Index));
            }
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {

            var roomTypes = await _roomTypeService.GetAllRoomTypesAsync();
            ViewBag.RoomTypes = new SelectList(roomTypes, "Id", "Type");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookingDto bookingDto)
        {
            var userId = _userManager.GetUserId(User);
            if (userId is not null)
            {
                string baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
                if (!ModelState.IsValid)
                {
                    var roomTypes = await _roomTypeService.GetAllRoomTypesAsync();
                    ViewBag.RoomTypes = new SelectList(roomTypes, "Id", "Type");
                    return View(bookingDto);
                }

                var createBookingResult = await _bookingService.CreateAsync(bookingDto, Guid.Parse(userId), baseUrl);

                if (createBookingResult.IsSuccess)
                {
                    var createdBookingDto = createBookingResult.Value;
                    var bills = await _billService.GetBillsByBookingId(createdBookingDto.Id);

                    if (bills == null || !bills.Any())
                    {
                        ModelState.AddModelError(string.Empty, "Problem while adding the bill for the particular booking!");
                        return View(bookingDto);
                    }

                    var firstBill = bills.FirstOrDefault();
                    if (firstBill != null)
                    {
                        return RedirectToAction("Details", "Bill", new { id = firstBill.Id });
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Problem while adding the bill for the particular booking!");
                        return View(bookingDto);
                    }
                }
                else
                {
                    var roomTypes = await _roomTypeService.GetAllRoomTypesAsync();
                    ViewBag.RoomTypes = new SelectList(roomTypes, "Id", "Type");
                    ModelState.AddModelError(string.Empty, createBookingResult.ErrorMessage);
                    return View(bookingDto);
                }
            }
            return View(bookingDto);
        }


        public async Task<IActionResult> Edit(Guid id)
        {
            try
            {
                var booking = await _bookingService.GetBookingByIdAsync(id);
                if (booking is null) return NotFound();

                var bookingDropDownData = await _bookingService.GetNewBookingDropDownsValues();
                if (bookingDropDownData is null) return BadRequest();

                ViewBag.Rooms = new SelectList(bookingDropDownData.Rooms, "Id", "RoomNumber");
                ViewBag.RoomTypes = new SelectList(bookingDropDownData.RoomTypes, "Id", "Type");
                booking.Rooms = bookingDropDownData
                                .Rooms;
                booking.RoomTypes = bookingDropDownData
                                .RoomTypes;
                return View("Edit", booking);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", ex.Message);
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, BookingDto bookingDto)
        {
            try
            {
                if (bookingDto.Id != id) return BadRequest();

                var booking = await _bookingService.GetBookingByIdAsync(id);
                if (booking is null) return NotFound();

                var bookingDropDownData = await _bookingService.GetNewBookingDropDownsValues();
                if (bookingDropDownData is null) return BadRequest();

                ViewBag.RoomTypes = new SelectList(bookingDropDownData.RoomTypes, "Id", "Type");
                ViewBag.Rooms = new SelectList(bookingDropDownData.Rooms, "Id", "RoomNumber");

                bookingDto.Rooms = bookingDropDownData
                                .Rooms;
                bookingDto.RoomTypes = bookingDropDownData
                                .RoomTypes;

                if (ModelState.IsValid)
                {
                    bookingDto.CreatedAt = booking.CreatedAt;
                    bookingDto.UpdatedAt = DateTime.Now;
                    var result = await _bookingService.UpdateAsync(id, bookingDto);
                    if (result != null && result.IsSuccess)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    ModelState.AddModelError(string.Empty, result.ErrorMessage);
                    ViewData["ErrorMessage"] = result.ErrorMessage;
                }
                return View(bookingDto);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", ex.Message);
                return View(bookingDto);
            }
        }


        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var booking = await _bookingService.GetBookingByIdAsync(id);
                return View("Delete", booking);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", ex.Message);
                return RedirectToAction(nameof(Index));
            }
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            try
            {
                await _bookingService.DeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", ex.Message);
                return RedirectToAction(nameof(Delete), new { id });
            }
        }
        public async Task<IActionResult> ViewBill(Guid id)
        {
            try
            {
                var bills = await _billService.GetBillsByBookingId(id);

                if (bills == null || !bills.Any())
                {
                    return NotFound();
                }

                var firstBill = bills.FirstOrDefault();
                if (firstBill != null)
                {
                    return RedirectToAction("Details", "Bill", new { id = firstBill.Id });
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", ex.Message);
                return RedirectToAction(nameof(Index));
            }
        }

        private async Task SetViewBagData()
        {
            var bookingDropDownData = await _bookingService.GetNewBookingDropDownsValues();
            ViewBag.Rooms = new SelectList(bookingDropDownData.Rooms, "Id", "RoomNumber");
            ViewBag.RoomTypes = new SelectList(bookingDropDownData.RoomTypes, "Id", "Type");
        }

        public async Task<IActionResult> BookingsChart()
        {
            var bookings = await _bookingService.GetBookingChartInfo();
            return PartialView("BookingsChart", bookings);

        }
    }
}