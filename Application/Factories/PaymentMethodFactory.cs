using ProvaPub.Application.Services;
using ProvaPub.Domain.Interfaces.Factories;
using ProvaPub.Domain.Interfaces.Services;

namespace ProvaPub.Application.Factories
{
    public class PaymentMethodFactory : IPaymentMethodFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public PaymentMethodFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IPaymentMethodService GetPaymentMethod(string paymentMethod)
        {
            return paymentMethod switch
            {
                "pix" => _serviceProvider.GetRequiredService<PixPaymentService>(),
                "creditcard" => _serviceProvider.GetRequiredService<CreditCardPaymentService>(),
                "paypal" => _serviceProvider.GetRequiredService<PayPalPaymentService>(),
                _ => throw new ArgumentException("Método de pagamento não reconhecido. Favor verificar."),
            };
        }
    }
}
