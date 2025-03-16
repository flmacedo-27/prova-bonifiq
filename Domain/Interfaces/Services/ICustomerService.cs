using ProvaPub.Application.Common.Models;
using ProvaPub.Domain.Models;

namespace ProvaPub.Domain.Interfaces.Services
{
    public interface ICustomerService
    {
        Task<PaginatedList<Customer>> ListCustomersAsync(int pageNumber, int pageSize);
        Task<Customer> CustomersAsync(int customerId);
        Task<bool> CanPurchase(int customerId, decimal purchaseValue);
    }
}
