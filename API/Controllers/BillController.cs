using Application.DTOs;
using Application.Services.BillService;
using Application.Services.BookingServices;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BillController : Controller
    {
        private readonly IBillService _billService;
        private readonly IBookingService _bookingService; // Inject the BookingService

        public BillController(IBillService billService, IBookingService bookingService)
        {
            _billService = billService;
            _bookingService = bookingService;
        }
        
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var bills=  await _billService.GetAllBills();
            return View("Index",bills);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var bill = await _billService.GetBillById(id);
            return View("Details",bill);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BillDto billDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Fetch the UserId from the associated BookingDto
                    var userId = await GetUserIdFromBookingAsync(billDto.BookingId);

                    if (userId == Guid.Empty)
                    {
                        return BadRequest("Invalid BookingId");
                    }

                    billDto.UserId = userId;

                    // Add the bill
                    await _billService.AddBill(billDto);

                    return RedirectToAction(nameof(Details), new { id = billDto.Id });
                }
                catch (Exception ex)
                {
                    return BadRequest($"Failed to add bill: {ex.Message}");
                }
            }

            // If the model state is not valid, return to the view with the original data
            return View(billDto);
        }

        // Helper method to retrieve UserId from associated BookingDto
        private async Task<Guid> GetUserIdFromBookingAsync(Guid bookingId)
        {
            var bookingDto = await _bookingService.GetBookingByIdAsync(bookingId);
            return bookingDto?.UserId ?? Guid.Empty;
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var billToUpdate = await _billService.GetBillById(id);
            return View(billToUpdate);
        }

        [HttpPost]
        public async Task<IActionResult> Update(BillDto billDto)
        {
            await _billService.UpdateBill(billDto);
            return RedirectToAction(nameof(Details), new { billDto.Id });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _billService.DeleteBill(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
