using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services.PaymentServices
{
    public interface IPaymentService
    {
        Task<PaymentDto> GetPaymentByIdAsync(Guid id);
        Task<IEnumerable<PaymentDto>> GetAllPaymentsAsync();
        Task AddPaymentAsync(PaymentDto paymentDto);
        Task UpdatePaymentAsync(Guid id, PaymentDto paymentDto);
        Task DeletePaymentAsync(Guid id);

    }
}
