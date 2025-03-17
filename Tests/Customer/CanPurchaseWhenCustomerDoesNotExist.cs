using ProvaPub.Application.Services;
using Xunit;

namespace ProvaPub.Tests.Customer
{
    public class CanPurchaseWhenCustomerDoesNotExist : CustomerTestBase
    {
        private readonly CustomerService _customerService;

        public CanPurchaseWhenCustomerDoesNotExist()
        {
            _customerService = new CustomerService(_context, _loggerMock.Object, _datetimeProviderMock.Object);
        }

        [Fact]
        public async Task CanPurchase_WhenCustomerDoesNotExist_Exception()
        {
            int customerId = 10;
            decimal purchaseValue = 50;

            await Assert.ThrowsAsync<InvalidOperationException>(() => _customerService.CanPurchase(customerId, purchaseValue));
        }
    }
}
