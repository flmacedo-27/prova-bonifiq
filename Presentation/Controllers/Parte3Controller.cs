using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProvaPub.Domain.Interfaces.Services;
using ProvaPub.Presentation.DataObjects;

namespace ProvaPub.Presentation.Controllers
{
    [ApiController]
	[Route("[controller]")]
	public class Parte3Controller : ControllerBase
	{
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public Parte3Controller(IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }

        [HttpGet("orders")]
        public async Task<OrderDto> PlaceOrder(string paymentMethod, decimal paymentValue, int customerId)
        {
            var order = await _orderService.PayOrder(paymentMethod, paymentValue, customerId);
            order.OrderDate = TimeZoneInfo.ConvertTimeFromUtc(order.OrderDate, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time"));
            var orderDto = _mapper.Map<OrderDto>(order);
            
            return orderDto;
        }
    }
}
