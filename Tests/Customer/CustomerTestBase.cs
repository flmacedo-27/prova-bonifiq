using Microsoft.EntityFrameworkCore;
using Moq;
using ProvaPub.Application.Common.Interfaces;
using ProvaPub.Application.Services;
using ProvaPub.Infrastructure.Data;

namespace ProvaPub.Tests.Customer
{
    public class CustomerTestBase
    {
        protected readonly TestDbContext _context;
        protected readonly Mock<ILogger<CustomerService>> _loggerMock;
        protected readonly Mock<IDateTimeProvider> _datetimeProviderMock;

        public CustomerTestBase()
        {
            var options = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new TestDbContext(options);
            _loggerMock = new Mock<ILogger<CustomerService>>();
            _datetimeProviderMock = ObterMockDateTimeProvider();

            InitializeDatabaseCustomer();
            InitializeDatabaseOrder();
        }

        protected virtual void InitializeDatabaseCustomer()
        {
            _context.Customers.Add(new Domain.Models.Customer { Id = 1, Name = "Customer Test 1" });
            _context.SaveChanges();
        }

        protected virtual void InitializeDatabaseOrder()
        {
            _context.Orders.Add(new Domain.Models.Order { Id = 1, CustomerId = 1, OrderDate = DateTime.Now });
            _context.SaveChanges();
        }

        protected virtual Mock<IDateTimeProvider> ObterMockDateTimeProvider()
        {
            var mock = new Mock<IDateTimeProvider>();

            mock.Setup(provider => provider.UtcNow).Returns(DateTime.Now);

            return mock;
        }
    }
}