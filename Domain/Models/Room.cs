using Domain.Base;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Room : BaseEntity
    {
        
        public int RoomNumber { get; set; }
        public bool IsFree { get; set; }


       public TypeOfService TypeOfService { get; set; }


        public Guid RoomTypeId { get; set; }
        [ForeignKey("RoomTypeId")]
       public RoomType RoomType { get; set; }
    }
}
