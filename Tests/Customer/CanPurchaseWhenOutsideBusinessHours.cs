using Moq;
using ProvaPub.Application.Common.Interfaces;
using ProvaPub.Application.Services;
using Xunit;

namespace ProvaPub.Tests.Customer
{
    public class CanPurchaseWhenOutsideBusinessHours : CustomerTestBase
    {
        private readonly CustomerService _customerService;

        public CanPurchaseWhenOutsideBusinessHours()
        {
            _customerService = new CustomerService(_context, _loggerMock.Object, _datetimeProviderMock.Object);
        }

        [Fact]
        public async Task CanPurchase_WhenOutsideBusinessHours_ReturnFalse()
        {
            int customerid = 1;
            decimal purchasevalue = 50;

            var result = await _customerService.CanPurchase(customerid, purchasevalue);

            Assert.False(result);
        }

        protected override Mock<IDateTimeProvider> ObterMockDateTimeProvider()
        {
            var mock = new Mock<IDateTimeProvider>();

            mock.Setup(provider => provider.UtcNow).Returns(new DateTime(2023, 12, 25, 12, 0, 0));

            return mock;
        }
    }
}
