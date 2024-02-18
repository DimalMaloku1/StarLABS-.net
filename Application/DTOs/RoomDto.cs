using Application.Base;
using Domain.Base;
using Domain.Enums;
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
        public bool IsFree { get; set; }
        public TypeOfService TypeOfService { get; set; }

        public Guid RoomTypeId { get; set; }
        public IEnumerable<RoomTypeDto> RoomTypes { get; set; }

        public string Type { get; set; }


    }
}
