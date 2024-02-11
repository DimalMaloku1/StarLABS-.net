using Domain.Base;
using Domain.Enums; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Payment : BaseEntity
    {
        public bool IsPaid { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public Bill Bill { get; set; }
        public Guid BillId { get; set; }
    }
}
