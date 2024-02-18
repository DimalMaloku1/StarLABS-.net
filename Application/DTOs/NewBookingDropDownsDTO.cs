using Domain.Models;

namespace Application.DTOs
{
    public record NewBookingDropDownsDTO
    {
        public NewBookingDropDownsDTO()
        {
            RoomTypes = Enumerable.Empty<RoomType>();
        }

        public IEnumerable<RoomType> RoomTypes { get; init; }
    }
}
