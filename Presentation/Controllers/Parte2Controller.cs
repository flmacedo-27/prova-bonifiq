using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProvaPub.Application.Common.Models;
using ProvaPub.Domain.Interfaces.Services;
using ProvaPub.Presentation.DataObjects;

namespace ProvaPub.Presentation.Controllers
{
    [ApiController]
	[Route("[controller]")]
	public class Parte2Controller :  ControllerBase
	{
        private readonly IProductService _productService;
        private readonly ICustomerService _customerService;
        private readonly IMapper _mapper;

        public Parte2Controller(IProductService productService, ICustomerService customerService, IMapper mapper)
        {
            _productService = productService;
            _customerService = customerService;
            _mapper = mapper;
        }

        [HttpGet("products")]
        public async Task<PaginatedList<ProductDto>> ListProducts(int pageNumber = 1, int pageSize = 10)
        {
            var products = await _productService.ListProductsAsync(pageNumber, pageSize);
            var productsDtos = _mapper.Map<PaginatedList<ProductDto>>(products);

            return productsDtos;
        }

        [HttpGet("customers")]
        public async Task<PaginatedList<CustomerDto>> ListCustomers(int pageNumber = 1, int pageSize = 10)
        {
            var customers = await _customerService.ListCustomersAsync(pageNumber, pageSize);
            var customersDtos = _mapper.Map<PaginatedList<CustomerDto>>(customers);
            
            return customersDtos;
        }
    }
}
