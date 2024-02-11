using Application.Base;
using Application.DTOs.Enums;
using System;

namespace Application.DTOs
{
    public class PaymentDto : BaseEntityDto
    {
        public bool IsPaid { get; set; }
        public PaymentMethodDto PaymentMethodDto { get; set; }
        public Guid BillId { get; set; }
    }
}
