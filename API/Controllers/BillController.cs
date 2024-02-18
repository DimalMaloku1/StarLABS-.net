using Application.DTOs;
using Application.Services.BillService;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BillController : Controller
    {
        private readonly IBillService _billService;
        public BillController(IBillService billService)
        {
            _billService = billService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var bills = await _billService.GetAllBills();
            return View("Index", bills);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var bill = await _billService.GetBillById(id);
            return View("Details", bill);
        }

        [HttpPost]
        public async Task<IActionResult> Create(BillDto billDto)
        {
            await _billService.AddBill(billDto);
            return RedirectToAction(nameof(Details), new { id = billDto.Id });
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