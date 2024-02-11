using Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Staff : BaseEntity
    {
        public string Department { get; set; }
        public decimal Salary { get; set; }
        public string Address { get; set; }

       // public string UserId { get; set; }
        public int PositionId { get; set; }
       // public IdentityUser User { get; set; }
        public Position Position { get; set; }
    }
}
