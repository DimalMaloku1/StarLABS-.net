using System;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Services.FeedbackServices;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class FeedbackController : Controller
    {
        private readonly IFeedbackService _feedbackService;

        public FeedbackController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        public async Task<IActionResult> Index()
        {
            var feedbacks = await _feedbackService.GetAllFeedbacksAsync();
            return View("Index", feedbacks);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var feedback = await _feedbackService.GetFeedbackByIdAsync(id);
            return View("Details", feedback);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(FeedbackDto feedbackDto)
        {
            await _feedbackService.CreateAsync(feedbackDto);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var feedback = await _feedbackService.GetFeedbackByIdAsync(id);
            return View("Edit", feedback);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, FeedbackDto feedbackDto)
        {
            await _feedbackService.UpdateAsync(id, feedbackDto);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var feedback = await _feedbackService.GetFeedbackByIdAsync(id);
            return View("Delete", feedback);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _feedbackService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
