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
            CreateMap<Product, ProductDto>();
            CreateMap<Order, OrderResponseDto>();                
            CreateMap<OrderDetail ,OrderDetailResponseDto> ();
        }
    }
}
