using Domain.Base;


namespace Domain.Models
{
    public class Bill : BaseEntity
    {
        public double TotalAmount { get; set; }
        public Guid BookingId { get; set; }
        public Booking Booking { get; set; }
    }
}
