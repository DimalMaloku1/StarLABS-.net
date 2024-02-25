using Domain.Models;

namespace Application.DTOs
{
    public record NewBookingDropDownsDTO
    {
        public NewBookingDropDownsDTO()
        {
            Rooms = Enumerable.Empty<RoomDto>();
            RoomTypes = Enumerable.Empty<RoomTypeDto>();
        }

        public IEnumerable<RoomTypeDto> RoomTypes { get; set; }
        public IEnumerable<RoomDto> Rooms { get; set; }
    }
}
