using ProvaPub.Application.Services;
using Xunit;

namespace ProvaPub.Tests.Customer
{
    public class CanPurchaseWhenFirstPurchaseExceeds100 : CustomerTestBase
    {
        private readonly CustomerService _customerService;

        public CanPurchaseWhenFirstPurchaseExceeds100()
        {
            _customerService = new CustomerService(_context, _loggerMock.Object, _datetimeProviderMock.Object);
        }

        [Fact]
        public async Task CanPurchase_WhenFirstPurchaseExceeds100_ReturnFalse()
        {
            int customerId = 1;
            decimal purchaseValue = 150;

            var result = await _customerService.CanPurchase(customerId, purchaseValue);

            Assert.False(result);
        }
    }
}
