using AutoMapper;
using ProniaOnion.Application.Dtos;
using ProniaOnion.Domain.Entities;

namespace ProniaOnion.Application.MappingProfiles
{
    internal class ProductProfile:Profile
    {
        public ProductProfile()
        {
            CreateMap<ProductPostDto, Product>();
            CreateMap<Product, ProductGetDto>();
            CreateMap<Product, ProductGetItemDto>();
            CreateMap<ProductPutDto, Product>();
        }
    }
}
