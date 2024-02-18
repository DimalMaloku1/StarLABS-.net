using Application.Base;
using Domain.Models;

namespace Application.DTOs
{
    public class BookingDto : BaseEntityDto
    {
        public int TotalPrice { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public Guid RoomId { get; set; }
        public Guid UserId { get; set; }
        // dto doesnt contain the needed information.
        public RoomDto Room { get; set; }
        public AppUser User { get; set; }
        public IEnumerable<RoomTypeDto> RoomTypes { get; set; }
    }
}
