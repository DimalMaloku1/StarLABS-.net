using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class BookingChartDto
    {
        public DateTime BookingDate { get; set; }
        public string? RoomType { get; set; }
    }
}
