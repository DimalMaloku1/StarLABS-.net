namespace Application.DTOs;

public class BookingConfirmationDto
{
    public Guid BookingId { get; set; }
    public Guid? UserId { get; set; }
    public Guid? BillId { get; set; }
    public string Username { get; set; }
    public DateTime? CheckInDate { get; set; }
    public DateTime? CheckOutDate { get; set; }
    public int? RoomNumber { get; set; }
    public string RoomType { get; set; }
    public double? TotalAmount { get; set; }
    public int? DaysSpent { get; set; }
    public string BillDetailsUrl { get; set; }
}