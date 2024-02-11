using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Room
    {
        public Guid Id { get; set; }
        public int RoomNumber { get; set; }
        public bool Status { get; set; }

        public RoomType RoomType { get; set; }
        public int RoomTypeID { get; set; } 

    }
}
