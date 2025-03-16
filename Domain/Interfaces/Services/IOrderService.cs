using ProvaPub.Domain.Models;

namespace ProvaPub.Domain.Interfaces.Services
{
    public interface IOrderService
    {
        Task<Order> PayOrder(string paymentMethod, decimal paymentValue, int customerId);
    }
}
