using Application.Base;
using Microsoft.AspNetCore.Http;

namespace Application.DTOs
{
    public class RoomTypeDto : BaseEntityDto
    {
        public string Type { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int Capacity { get; set; }
        public List<RoomTypePhotoDTO> Photos { get; set; }
    }
}
