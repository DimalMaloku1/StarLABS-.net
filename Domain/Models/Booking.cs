using Domain.Base;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Domain.Models
{
    public class Booking : BaseEntity
    {
        private int _totalPrice;
        private DateTime _checkInDate;
        private DateTime _checkOutDate;
        private Guid _roomId;
        private Guid _userId;
        private Guid _roomTypeId;

        public Booking(int totalPrice, DateTime checkInDate, DateTime checkOutDate, Guid roomId, Guid userId, Guid roomTypeId)
        {
            TotalPrice = totalPrice;
            CheckInDate = checkInDate;
            CheckOutDate = checkOutDate;
            RoomId = roomId;
            UserId = userId;
            RoomTypeId = roomTypeId;
        }

        public int TotalPrice
        {
            get => _totalPrice;
            private set => _totalPrice = value;
        }

        public DateTime CheckInDate
        {
            get => _checkInDate;
            private set => _checkInDate = value;
        }
        public DateTime CheckOutDate
        {
            get => _checkOutDate;
            private set => _checkOutDate = value;
        }

        public Guid RoomId
        {
            get => _roomId;
            set => _roomId = value;
        }

        public Guid UserId
        {
            get => _userId;
            set => _userId = value;
        }
        public Guid RoomTypeId
        {
            get => _roomTypeId;
            set => _roomTypeId = value;
        }
        [JsonIgnore]
        public RoomType RoomType { get; set; }

        [ForeignKey("RoomId")]
        public Room Room { get; set; }
        [ForeignKey("UserId")]
        public AppUser User { get; set; }

        //public List<Booking_Room> Bookings_Rooms { get; set; }


    }
}
