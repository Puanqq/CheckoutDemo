using AutoMapper;
using Checkout.API.DTOs;
using Checkout.Entities.Models;
using System.Linq;

namespace Checkout.API.Mappings
{
    public class DomainToResponseProfile : Profile
    {
        public DomainToResponseProfile()
        {
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.ProductName, opt =>
                    opt.MapFrom(src => src.PName))
                .ForMember(dest => dest.ProductPrice, opt =>
                    opt.MapFrom(src => src.Price))
                .ReverseMap();            

            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.DetailOder, opt =>
                    opt.MapFrom(src => src.Details))
                .ReverseMap();                

            CreateMap<OrderDetail ,OrderDetailDto>()
                .ReverseMap();
        }
    }
}
