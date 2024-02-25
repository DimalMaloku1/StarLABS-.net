using Application.Base;
using Domain.Models;

namespace Application.DTOs
{

    public class BillDto : BaseEntityDto
    {
        public Guid BookingId { get; set; }
        public Guid UserId { get; set; }
        public string? Username { get; set; }
        public decimal TotalAmount { get; set; }
        public Booking Booking { get; set; }
        public int? DaysSpent { get; set; }
        public string? RoomType { get; set; }
        public double? RoomPrice { get; set; }
        public IEnumerable<Booking> BookingList { get; set; }

    }
}