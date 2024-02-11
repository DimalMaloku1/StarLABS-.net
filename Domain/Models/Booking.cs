using Domain.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    public class Booking : BaseEntity
    {
        public int TotalPrice { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public Guid RoomId { get; set; }
        [ForeignKey("RoomId")]
        public Room Room { get; set; }
    }
}
