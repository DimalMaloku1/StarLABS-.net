using Domain.Base;
using Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    public class Payment : BaseEntity
    {
        public PaymentMethod PaymentMethod { get; set; }
        public Guid BillId { get; set; }
        [ForeignKey("BillId")]
        public Bill Bill { get; set; }
        public double TotalAmount { get; set; }
    }
}
