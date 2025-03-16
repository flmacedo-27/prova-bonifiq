using ProvaPub.Application.Common.Models;
using ProvaPub.Domain.Models;

namespace ProvaPub.Domain.Interfaces.Services
{
    public interface IProductService
    {
        Task<PaginatedList<Product>> ListProductsAsync(int pageNumber, int pageSize);
    }
}
