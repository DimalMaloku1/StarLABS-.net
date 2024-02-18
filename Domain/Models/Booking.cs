using Domain.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    public class Booking : BaseEntity
    {
        private int _totalPrice;
        private DateTime _checkInDate;
        private DateTime _checkOutDate;
        private Guid _roomId;
        private Guid _userId;

        public Booking(int totalPrice, DateTime checkInDate, DateTime checkOutDate, Guid roomId, Guid userId)
        {
            TotalPrice = totalPrice;
            CheckInDate = checkInDate;
            CheckOutDate = checkOutDate;
            RoomId = roomId;
            UserId = userId;
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
            private set => _roomId = value;
        }

        public Guid UserId
        {
            get => _userId;
            private set => _userId = value;
        }
        [ForeignKey("RoomId")]
        public Room Room { get; set; }
        [ForeignKey("UserId")]
        public AppUser User { get; set; }



    }
}
