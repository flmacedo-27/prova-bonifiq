using ProvaPub.Application.Services;
using Xunit;

namespace ProvaPub.Tests.Customer
{
    public class CanPurchaseWhenCustomerIdIsInvalid : CustomerTestBase
    {
        private readonly CustomerService _customerService;

        public CanPurchaseWhenCustomerIdIsInvalid()
        {
            _customerService = new CustomerService(_context, _loggerMock.Object, _datetimeProviderMock.Object);
        }

        [Fact]
        public async Task CanPurchase_WhenCustomerIdIsInvalid_Exception()
        {
            int customerId = 0;
            decimal purchaseValue = 50;

            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => _customerService.CanPurchase(customerId, purchaseValue));
        }
    }
}
