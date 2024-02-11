using Application.Base;

namespace Application.DTOs
{
    public class BookingDto : BaseEntityDto
    {
        public int TotalPrice { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public Guid RoomId { get; set; }
    }
}
