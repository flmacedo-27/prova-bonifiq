using ProvaPub.Domain.Interfaces.Services;

namespace ProvaPub.Domain.Interfaces.Factories
{
    public interface IPaymentMethodFactory
    {
        IPaymentMethodService GetPaymentMethod(string paymentMethod);
    }
}
