using Application.DTOs;
using Application.Services.BillService;
using Application.Services.BookingServices;
using Domain.Models;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace API.Controllers
{
    [Authorize]
    public class BookingsController(UserManager<AppUser> _userManager, IBookingService _bookingService, IValidator<BookingDto> _bookingValidator, IBillService _billService) : Controller
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
                await SetViewBagData();
                return View("Details", booking);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", ex.Message);
                return RedirectToAction(nameof(Index));
            }
        }
        public async Task<IActionResult> Create()
        {
            try
            {
                await SetViewBagData();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", ex.Message);
                return View("Create");
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookingDto bookingDto)
        {
            try
            {
                var userId = _userManager.GetUserId(User);
                if (userId is not null)
                {
                    await SetViewBagData();
                    string baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";

                    var validationResult = _bookingValidator.Validate(bookingDto);
                    if (validationResult.IsValid)
                    {
                        var createdBookingDto = await _bookingService.CreateAsync(bookingDto, Guid.Parse(userId), baseUrl);
                        var bills = await _billService.GetBillsByBookingId(createdBookingDto.Id);

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
                    else
                    {
                        foreach (var error in validationResult.Errors)
                        {
                            ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                        }
                    }
                }
                else
                {
                    return RedirectToAction(nameof(Index));
                }
                return View(bookingDto);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", ex.Message);
                return View(bookingDto);
            }
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

                var validationResult = _bookingValidator.Validate(bookingDto);
                if (validationResult.IsValid)
                {
                    bookingDto.CreatedAt = booking.CreatedAt;
                    bookingDto.UpdatedAt = DateTime.Now;
                    await _bookingService.UpdateAsync(id, bookingDto);
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
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
                await SetViewBagData();
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

    }
}