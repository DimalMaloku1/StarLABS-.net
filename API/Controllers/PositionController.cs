using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Application.DTOs;
using Application.Services.PositionServices;
using Application.Enums;

namespace YourNamespace.Controllers
{
    public class PositionController : Controller
    {
        private readonly IPositionServices _positionServices;

        public PositionController(IPositionServices positionServices)
        {
            _positionServices = positionServices;
        }

        public async Task<ActionResult> Index()
        {
            var positions = await _positionServices.GetAllPositionsAsync();
            return View(positions);
        }

        public async Task<ActionResult> Details(Guid id)
        {
            var position = await _positionServices.GetPositionByIdAsync(id);
            if (position == null)
            {
                return NotFound();
            }
            return View(position);
        }

        public ActionResult Create()
        {
            ViewBag.ShiftOptions = new SelectList(Enum.GetValues(typeof(Shiftdto)));
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(PositionDTO positionDto)
        {
            if (ModelState.IsValid)
            {
                await _positionServices.AddPositionAsync(positionDto);
                return RedirectToAction("Index");
            }

            return View(positionDto);
        }

        public async Task<ActionResult> Edit(Guid id)
        {
            var position = await _positionServices.GetPositionByIdAsync(id);
            if (position == null)
            {
                return NotFound();
            }
            return View(position);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Guid id, PositionDTO positionDto)
        {
            if (ModelState.IsValid)
            {
                  await _positionServices.UpdatePositionAsync(id, positionDto);
                    return RedirectToAction("Index");
            }
            return View(positionDto);
        }

        public async Task<ActionResult> Delete(Guid id)
        {
            await _positionServices.DeletePositionAsync(id);
            return RedirectToAction("Index");
        }
    }
}
