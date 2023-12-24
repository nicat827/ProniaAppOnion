using AutoMapper;
using ProniaOnion.Application.Dtos;
using ProniaOnion.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProniaOnion.Application.MappingProfiles
{
    internal class CategoryProfile:Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category,CategoryGetItemDto>();
            CreateMap<CategoryPostDto, Category>();
            CreateMap<CategoryPutDto, Category>();
            CreateMap<Category, CategoryGetDto>();
        }
    }
}
