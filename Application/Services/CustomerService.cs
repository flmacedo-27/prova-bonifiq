using Microsoft.EntityFrameworkCore;
using ProvaPub.Application.Common.Mappings;
using ProvaPub.Application.Common.Models;
using ProvaPub.Domain.Interfaces.Services;
using ProvaPub.Domain.Models;
using ProvaPub.Infrastructure.Data;
using System.Text.Json;

namespace ProvaPub.Application.Services
{
    public class CustomerService : BaseService<Customer>, ICustomerService
    {
        public CustomerService(TestDbContext ctx, ILogger<CustomerService> logger) : base(ctx, logger) { }

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
                    throw new ArgumentOutOfRangeException(nameof(customerId));
                }

                var customer = await _ctx.Customers.FindAsync(customerId);

                if (customer == null)
                {
                    _logger.LogWarning("Customer Id {CustomerId} does not exists", customerId);
                    throw new InvalidOperationException($"Customer Id {customerId} does not exists");
                }

                string customerJson = JsonSerializer.Serialize(customer, new JsonSerializerOptions { WriteIndented = true });
                _logger.LogInformation("Returning customer {CustomerJson}", customerJson);

                return customer;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching the customer from the database");
                return new Customer();
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
                    throw new ArgumentOutOfRangeException(nameof(purchaseValue));
                }

                var customer = await CustomersAsync(customerId);

                //Business Rule: A customer can purchase only a single time per month
                var baseDate = DateTime.UtcNow.AddMonths(-1);
                var ordersInThisMonth = await _ctx.Orders.CountAsync(s => s.CustomerId == customerId && s.OrderDate >= baseDate);
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
                if (DateTime.UtcNow.Hour < 8 || DateTime.UtcNow.Hour > 18 || DateTime.UtcNow.DayOfWeek == DayOfWeek.Saturday || DateTime.UtcNow.DayOfWeek == DayOfWeek.Sunday)
                {
                    _logger.LogWarning("A customer {CustomerName} can purchases only during business hours and working days", customer.Name);
                    return false;
                }

                _logger.LogInformation("A customer {CustomerName} can purchase", customer.Name);

                return true;
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error while fetching order or customer information from the database");
                return false;
            }
        }
    }
}
