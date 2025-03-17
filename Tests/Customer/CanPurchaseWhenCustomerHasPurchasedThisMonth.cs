using ProvaPub.Application.Services;
using Xunit;

namespace ProvaPub.Tests.Customer
{
    public class CanPurchaseWhenCustomerHasPurchasedThisMonth : CustomerTestBase
    {
        private readonly CustomerService _customerService;

        public CanPurchaseWhenCustomerHasPurchasedThisMonth()
        {
            _customerService = new CustomerService(_context, _loggerMock.Object, _datetimeProviderMock.Object);
        }

        [Fact]
        public async Task CanPurchase_WhenCustomerHasPurchasedThisMonth_ReturnFalse()
        {
            int customerId = 1;
            decimal purchaseValue = 50;

            var result = await _customerService.CanPurchase(customerId, purchaseValue);

            Assert.False(result);
        }
    }
}
