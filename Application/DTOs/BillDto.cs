using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class BillDto
    {
        public Guid Id { get; set; }
        public Guid BookingId { get; set; }
        public Guid UserId { get; set; }
        public string Username { get; set; }

        public decimal TotalAmount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}