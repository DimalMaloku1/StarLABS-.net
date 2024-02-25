using Application.DTOs;
using Application.Services.BookingServices;
using Application.Services.RoomServices;
using Application.Validations;
using Domain.Models;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace API.Controllers
{
    public class BookingsController(UserManager<AppUser> _userManager, IBookingService _bookingService, IValidator<BookingDto> _bookingValidator) : Controller
    {

        private async Task SetViewBagData()
        {
            var bookingDropDownData = await _bookingService.GetNewBookingDropDownsValues();
            ViewBag.Rooms = new SelectList(bookingDropDownData.Rooms, "Id", "RoomNumber");
            ViewBag.RoomTypes = new SelectList(bookingDropDownData.RoomTypes, "Id", "Type");
        }

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
                if (userId != null)
                {
                    await SetViewBagData();
                    var validationResult = _bookingValidator.Validate(bookingDto);
                    if (validationResult.IsValid)
                    {
                        await _bookingService.CreateAsync(bookingDto, Guid.Parse(userId));
                        return RedirectToAction(nameof(Index));
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
    }
}
