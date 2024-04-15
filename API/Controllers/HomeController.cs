using Microsoft.AspNetCore.Mvc;
using YourProject.ViewModels; 
using Application.Services.RoomServices;
using Application.Services.RoomTypeServices;
using Application.Services.FeedbackServices;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly IRoomTypeServices _roomTypeService;
        private readonly IRoomServices _roomService;
        private readonly IFeedbackService _feedbackService;

        public HomeController(IRoomTypeServices roomTypeService, IRoomServices roomService, IFeedbackService feedbackService)
        {
            _roomTypeService = roomTypeService;
            _roomService = roomService;
            _feedbackService = feedbackService;
        }

        public async Task<IActionResult> Indexx()
        {
            var roomTypes = await _roomTypeService.GetAllRoomTypesAsync();
            var feedbacks = await _feedbackService.GetAllFeedbacksAsync();

            var viewModel = new HomePageViewModel
            {
                RoomTypes = roomTypes,
                Feedbacks = feedbacks,
                AverageRating = await _feedbackService.CalculateAverageRatingAsync()
            };

            return View(viewModel);
        }
    }
}
