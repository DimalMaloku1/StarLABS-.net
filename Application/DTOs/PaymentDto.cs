using Application.Base;
using Domain.Enums;

namespace Application.DTOs
{
    public class PaymentDto : BaseEntityDto
    {
        public PaymentMethod PaymentMethod { get; set; }
        public Guid BillId { get; set; }
        public string Username { get; set; }
        public decimal TotalAmount { get; set; }
        public IEnumerable<BillDto> Bills { get; set; }
    }
}
