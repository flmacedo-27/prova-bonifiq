﻿using ProvaPub.Domain.Interfaces.Services;

namespace ProvaPub.Application.Services
{
    public class CreditCardPaymentService : IPaymentMethodService
    {
        public async Task<bool> ProcessPayment(decimal paymentValue)
        {
            return await Task.FromResult(true);
        }
    }
}
