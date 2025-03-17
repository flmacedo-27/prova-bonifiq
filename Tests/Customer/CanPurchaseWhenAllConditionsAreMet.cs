using ProvaPub.Application.Services;
using Xunit;

namespace ProvaPub.Tests.Customer
{
    public class CanPurchaseWhenAllConditionsAreMet : CustomerTestBase
    {
        private readonly CustomerService _customerService;

        public CanPurchaseWhenAllConditionsAreMet()
        {
            _customerService = new CustomerService(_context, _loggerMock.Object, _datetimeProviderMock.Object);
        }

        [Fact]
        public async Task CanPurchase_WhenAllConditionsAreMet_ReturnTrue()
        {
            int customerId = 1;
            decimal purchaseValue = 50;

            var result = await _customerService.CanPurchase(customerId, purchaseValue);

            Assert.True(result);
        }

        protected override void InitializeDatabaseOrder()
        {
            _context.Orders.Add(new Domain.Models.Order { Id = 1, CustomerId = 1, OrderDate = DateTime.Now.AddMonths(-3) });
            _context.SaveChanges();
        }
    }
}
