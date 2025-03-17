using ProvaPub.Application.Services;
using Xunit;

namespace ProvaPub.Tests.Customer
{
    public class CanPurchaseWhenPurchaseValueIsInvalid : CustomerTestBase
    {
        private readonly CustomerService _customerService;

        public CanPurchaseWhenPurchaseValueIsInvalid()
        {
            _customerService = new CustomerService(_context, _loggerMock.Object, _datetimeProviderMock.Object);
        }

        [Fact]
        public async Task CanPurchase_WhenPurchaseValueIsInvalid_Exception()
        {
            int customerId = 1;
            decimal purchaseValue = 0;

            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => _customerService.CanPurchase(customerId, purchaseValue));
        }
    }
}
