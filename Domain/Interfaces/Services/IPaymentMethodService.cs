namespace ProvaPub.Domain.Interfaces.Services
{
    public interface IPaymentMethodService
    {
        Task<bool> ProcessPayment(decimal paymentValue);
    }
}
