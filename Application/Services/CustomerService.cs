using Microsoft.EntityFrameworkCore;
using ProvaPub.Application.Common.Interfaces;
using ProvaPub.Application.Common.Mappings;
using ProvaPub.Application.Common.Models;
using ProvaPub.Domain.Interfaces.Services;
using ProvaPub.Domain.Models;
using ProvaPub.Infrastructure.Data;

namespace ProvaPub.Application.Services
{
    public class CustomerService : BaseService<Customer>, ICustomerService
    {
        private readonly IDateTimeProvider _dateTimeProvider;

        public CustomerService(TestDbContext ctx, ILogger<CustomerService> logger, IDateTimeProvider dateTimeProvider) : base(ctx, logger) 
        {
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<PaginatedList<Customer>> ListCustomersAsync(int pageNumber, int pageSize)
        {
            _logger.LogInformation("Searching for customer list");

            var customers = await _ctx.Customers
                .AsNoTracking()
                .PaginatedListAsync(pageNumber, pageSize);

            _logger.LogInformation("Returning {CustomersCount} products", customers.Items.Count);

            return customers;
        }

        public async Task<Customer> CustomersAsync(int customerId)
        {
            try
            {
                _logger.LogInformation("Searching for customer with Id {CustomerId}", customerId);

                if (customerId <= 0)
                {
                    _logger.LogWarning("Customer with Id {CustomerId} is less than 0", customerId);
                    throw new ArgumentOutOfRangeException($"Customer with Id {customerId} is less than 0");
                }

                var customer = await _ctx.Customers.FindAsync(customerId);

                if (customer == null)
                {
                    _logger.LogWarning("Customer Id {CustomerId} does not exists", customerId);
                    throw new InvalidOperationException($"Customer Id {customerId} does not exists");
                }

                _logger.LogInformation("Returning customer {CustomerId}", customer.Id);

                return customer;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error CustomersAsync");
                throw;
            }
        }

        public async Task<bool> CanPurchase(int customerId, decimal purchaseValue)
        {
            try
            {
                _logger.LogInformation("Checking can purchase in customer id {CustomerId}", customerId);

                if (purchaseValue <= 0)
                {
                    _logger.LogWarning("Purchase value {PurchaseValue} is less than 0", purchaseValue);
                    throw new ArgumentOutOfRangeException($"Purchase value {purchaseValue} is less than 0");
                }

                var customer = await CustomersAsync(customerId);

                var baseDate = _dateTimeProvider.UtcNow;

                //Business Rule: A customer can purchase only a single time per month
                var lastMonth = _dateTimeProvider.UtcNow.AddMonths(-1);
                var ordersInThisMonth = await _ctx.Orders.CountAsync(s => s.CustomerId == customerId && s.OrderDate >= lastMonth);
                if (ordersInThisMonth > 0)
                {
                    _logger.LogWarning("A customer {CustomerName} can purchase only a single time per month", customer.Name);
                    return false;
                }

                //Business Rule: A customer that never bought before can make a first purchase of maximum 100,00
                var haveBoughtBefore = await _ctx.Customers.CountAsync(s => s.Id == customerId && s.Orders.Any());
                if (haveBoughtBefore == 0 && purchaseValue > 100)
                {
                    _logger.LogWarning("A customer {CustomerName} that never bought before can make a first purchase of maximum 100,00", customer.Name);
                    return false;
                }
                
                //Business Rule: A customer can purchases only during business hours and working days
                if (baseDate.Hour < 8 || baseDate.Hour > 18 || baseDate.DayOfWeek == DayOfWeek.Saturday || baseDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    _logger.LogWarning("A customer {CustomerName} can purchases only during business hours and working days", customer.Name);
                    return false;
                }

                _logger.LogInformation("A customer {CustomerName} can purchase", customer.Name);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error CanPurchase");
                throw;
            }
        }
    }
}
