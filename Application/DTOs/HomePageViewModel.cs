using System.Collections.Generic;
using Application.DTOs;

namespace YourProject.ViewModels
{
    public class HomePageViewModel
    {
        public IEnumerable<RoomTypeDto> RoomTypes { get; set; }
        public IEnumerable<FeedbackDto> Feedbacks { get; set; }
        public double AverageRating { get; set; } // Add this property
    }
}
