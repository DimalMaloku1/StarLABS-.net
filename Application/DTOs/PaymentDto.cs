using Application.Base;
using Domain.Enums;

namespace Application.DTOs
{
    public class PaymentDto : BaseEntityDto
    {
        public bool IsPaid { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public Guid BillId { get; set; }
        public IEnumerable<BillDto> Bills { get; set; }
    }
}
