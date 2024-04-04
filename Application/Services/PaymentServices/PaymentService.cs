using Application.DTOs;
using AutoMapper;
using Domain.Contracts;
using Domain.Models;

namespace Application.Services.PaymentServices
{
    public class PaymentService(IPaymentRepository _paymentRepository, IMapper _mapper) : IPaymentService
    {
        public async Task<PaymentDto> GetPaymentByIdAsync(Guid id)
        {
            var payment = await _paymentRepository.GetByIdAsync(id);
            return _mapper.Map<PaymentDto>(payment);
        }

        public async Task<IEnumerable<PaymentDto>> GetAllPaymentsAsync()
        {
            var payments = await _paymentRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<PaymentDto>>(payments);
        }

        public async Task AddPaymentAsync(PaymentDto paymentDto)
        {
            var payment = _mapper.Map<Payment>(paymentDto);
            await _paymentRepository.AddAsync(payment);
        }

        public async Task UpdatePaymentAsync(Guid id, PaymentDto paymentDto)
        {
            var existingPayment = await _paymentRepository.GetByIdAsync(id);
            existingPayment.PaymentMethod = paymentDto.PaymentMethod;
            _mapper.Map(paymentDto, existingPayment);
            await _paymentRepository.UpdateAsync(id, existingPayment);
        }

        public async Task DeletePaymentAsync(Guid id)
        {
            await _paymentRepository.DeleteAsync(id);
        }
    }
}
