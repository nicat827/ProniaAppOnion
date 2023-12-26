using AutoMapper;
using ProniaOnion.Application.Dtos;
using ProniaOnion.Application.Dtos.Color;
using ProniaOnion.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProniaOnion.Application.MappingProfiles
{
    internal class ColorProfile:Profile
    {
        public ColorProfile()
        {
            CreateMap<Color, ColorGetItemDto>();
            CreateMap<ColorPostDto, Color>();
            CreateMap<ColorPutDto, Color>();
            CreateMap<Color, ColorGetDto>();
        }
    }
}
