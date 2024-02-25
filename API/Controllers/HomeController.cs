using Application.Services.FeedbackServices;
using Microsoft.AspNetCore.Mvc;

public class HomeController : Controller
{
    private readonly IFeedbackService _feedbackService;

    public HomeController(IFeedbackService feedbackService)
    {
        _feedbackService = feedbackService;
    }

    public async Task<IActionResult> Index()
    {
        var topRatedFeedbacks = await _feedbackService.GetTopRatedFeedbacksAsync(3);
        return View(topRatedFeedbacks);
    }

    
}
