using Domain.Base;
using Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    public class Payment : BaseEntity
    {
        public bool IsPaid { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public Guid BillId { get; set; }
        public Bill Bill { get; set; }
    }
}
