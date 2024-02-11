using AutoMapper;
using Domain.Models;
using Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DTOs;
using Domain.Enums;

namespace Application.Services.PaymentServices
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IMapper _mapper;

        public PaymentService(IPaymentRepository paymentRepository, IMapper mapper)
        {
            _paymentRepository = paymentRepository;
            _mapper = mapper;
        }

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
            if (existingPayment == null)
            {
                return;
            }

            existingPayment.PaymentMethod = _mapper.Map<PaymentMethod>(paymentDto.PaymentMethodDto); 

            _mapper.Map(paymentDto, existingPayment);

            await _paymentRepository.UpdateAsync(id, existingPayment);
        }




        public async Task DeletePaymentAsync(Guid id)
        {
            await _paymentRepository.DeleteAsync(id);
        }
    }
}
