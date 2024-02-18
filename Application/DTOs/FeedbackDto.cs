using Application.Base;

namespace Application.DTOs
{
    public class FeedbackDto : BaseEntityDto
    {
        public string Comment { get; set; }
        public int Rating { get; set; }
    }
}
