using Application.Base;
using Domain.Models;

namespace Application.DTOs
{
    public class FeedbackDto : BaseEntityDto
    {
        public Guid UserId { get; set; }
        public AppUser User { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; }
    }
}
