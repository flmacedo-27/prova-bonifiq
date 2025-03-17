using Microsoft.EntityFrameworkCore;
using ProvaPub.Application.Common.Mappings;
using ProvaPub.Application.Common.Models;
using ProvaPub.Domain.Interfaces.Services;
using ProvaPub.Domain.Models;
using ProvaPub.Infrastructure.Data;

namespace ProvaPub.Application.Services
{
    public class ProductService: BaseService<Product>, IProductService
    {
        public ProductService(TestDbContext ctx, ILogger<ProductService> logger) : base(ctx, logger) { }

        public async Task<PaginatedList<Product>> ListProductsAsync(int pageNumber, int pageSize)
        {
            try
            {
                _logger.LogInformation("Searching for product list");

                var products = await _ctx.Products
                    .AsNoTracking()
                    .PaginatedListAsync(pageNumber, pageSize);

                _logger.LogInformation("Returning {ProductsCount} products", products.Items.Count);

                return products;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error ListProductsAsync");
                return new PaginatedList<Product>(
                    items: new List<Product>(),
                    count: 0,
                    pageIndex: pageNumber,
                    pageSize: pageSize
                );
            }
        }
	}
}