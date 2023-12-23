using AutoMapper;
using ProniaOnion.Application.Dtos.Tag;
using ProniaOnion.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProniaOnion.Application.MappingProfiles
{
    internal class TagProfile:Profile
    {
        public TagProfile()
        {
            CreateMap<TagCreateDto, Tag>();
            CreateMap<TagUpdateDto, Tag>();
            CreateMap<Tag, TagGetDto>();
            CreateMap<Tag, TagGetCollectionDto>();
        }
    }
}
