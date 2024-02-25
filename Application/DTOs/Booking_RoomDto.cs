using Application.Base;

namespace Application.DTOs
{
    public class Booking_RoomDto : BaseEntityDto
    {
        public Guid BookingId { get; set; }
        public Guid RoomId { get; set; }
    }
}
