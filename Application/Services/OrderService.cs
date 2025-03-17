using ProvaPub.Domain.Interfaces.Factories;
using ProvaPub.Domain.Interfaces.Services;
using ProvaPub.Domain.Models;
using ProvaPub.Infrastructure.Data;

namespace ProvaPub.Application.Services
{
    public class OrderService : BaseService<Order>, IOrderService
    {
        private readonly IPaymentMethodFactory _paymentMethodFactory;
        private readonly ICustomerService _customerService;

        public OrderService(
            TestDbContext ctx,
            ILogger<OrderService> logger,
            ICustomerService customerService,
            IPaymentMethodFactory paymentMethodFactory) : base(ctx, logger) 
        {
            _customerService = customerService;
            _paymentMethodFactory = paymentMethodFactory;
        }

        public async Task<Order> PayOrder(string paymentMethod, decimal paymentValue, int customerId)
        {
            try
            {
                _logger.LogInformation("Starting payment for customer with Id {CustomerId}", customerId);

                if (paymentValue <= 0)
                {
                    _logger.LogWarning("Payment value {PaymentValue} is less than 0", paymentValue);
                    throw new ArgumentOutOfRangeException(nameof(paymentValue));
                }

                var customer = await _customerService.CustomersAsync(customerId);

                _logger.LogInformation("Processing payment method {PaymentMethod}", paymentMethod);

                var paymentProcessor = _paymentMethodFactory.GetPaymentMethod(paymentMethod);
                var paymentSuccess = await paymentProcessor.ProcessPayment(paymentValue);

                _logger.LogInformation("Payment method completed");

                if (!paymentSuccess)
                {
                    _logger.LogWarning("Payment failed");
                    throw new InvalidOperationException("Payment failed. Please try again...");
                }

                var order = new Order
                {
                    CustomerId = customer.Id,
                    Value = paymentValue,
                    OrderDate = DateTime.UtcNow
                };

                await _ctx.Orders.AddAsync(order);
                await _ctx.SaveChangesAsync();

                _logger.LogInformation("Order saved successfully. CustomerId: {CustomerId}, Value: {Value}", order.CustomerId, order.Value);

                return order;
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error PayOrder");
                return new Order();
            }
        }
    }
}
