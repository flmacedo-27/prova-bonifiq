using AutoMapper;
using ProvaPub.Application.Common.Models;
using ProvaPub.Domain.Models;
using ProvaPub.Presentation.DataObjects;

namespace ProvaPub.Application.Common.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDto>();
            CreateMap<Customer, CustomerDto>();
            CreateMap<Order, OrderDto>();

            CreateMap<PaginatedList<Product>, PaginatedList<ProductDto>>()
                .ConstructUsing((src, ctx) =>
                {
                    var items = ctx.Mapper.Map<List<ProductDto>>(src.Items);
                    return new PaginatedList<ProductDto>(items, src.TotalCount, src.PageIndex, src.PageSize);
                });

            CreateMap<PaginatedList<Customer>, PaginatedList<CustomerDto>>()
                .ConstructUsing((src, ctx) =>
                {
                    var items = ctx.Mapper.Map<List<CustomerDto>>(src.Items);
                    return new PaginatedList<CustomerDto>(items, src.TotalCount, src.PageIndex, src.TotalPages);
                });
        }
    }
}
