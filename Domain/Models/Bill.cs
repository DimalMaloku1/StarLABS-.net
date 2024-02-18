using Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Bill : BaseEntity
    {
        public double TotalAmount { get; set; }
        // Changed the type of BookingId to Guid
         public Guid BookingId { get; set; }
        public Booking Booking { get; set; }
    }
}
