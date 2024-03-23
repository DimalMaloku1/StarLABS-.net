using Application.Base;
using Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class RoomDto : BaseEntityDto
    {
        public int RoomNumber { get; set; }

        public Guid RoomTypeId { get; set; }

        public string Type { get; set; }

        public string Description { get; set; }
        public double Price { get; set; }
        public int Capacity { get; set; }

        public List<BookingDto> BookingDtos { get; set; }
        public IEnumerable<RoomTypeDto> RoomTypes { get; set; }



    }
}
