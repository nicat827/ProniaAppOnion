using AutoMapper;
using ProniaOnion.Application.Dtos.AppUser;
using ProniaOnion.Domain.Entities;

namespace ProniaOnion.Application.MappingProfiles
{
    internal class AuthProfile:Profile
    {
        public AuthProfile()
        {
            CreateMap<RegisterDto, AppUser>();
            CreateMap<LoginDto, AppUser>();
            CreateMap<AppUser, AppUserGetItemDto>();
            CreateMap<AppUser, AppUserGetDto>();
        }
    }
}
