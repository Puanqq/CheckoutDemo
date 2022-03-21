using AutoMapper;
using Checkout.API.DTOs;
using Checkout.Entities.Models;

namespace Checkout.API.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ProductDto, Product>();
            CreateMap<Product, ProductDto>();
            CreateMap<OrderDto, Order>();
            CreateMap<OrderDetailDto, OrderDetail>();
        }
    }
}
