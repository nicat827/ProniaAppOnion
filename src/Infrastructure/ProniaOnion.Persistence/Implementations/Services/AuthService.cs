using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProniaOnion.Application.Abstractions.Services;
using ProniaOnion.Application.Dtos;
using ProniaOnion.Application.Dtos.AppUser;
using ProniaOnion.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProniaOnion.Persistence.Implementations.Services
{
    internal class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public AuthService(UserManager<AppUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }
        public async Task Register(RegisterDto dto)
        {
            if (await _userManager.Users.AnyAsync(u => u.UserName == dto.UserName || u.Email == dto.Email))
                throw new Exception("User with this email or username already exists!");
            AppUser user = _mapper.Map<AppUser>(dto);
            var res = await _userManager.CreateAsync(user, dto.Password);
            if (!res.Succeeded)
            {
                StringBuilder sb = new StringBuilder();
                foreach (IdentityError err in res.Errors)
                {
                    sb.AppendLine(err.Description);
                }
                throw new Exception(sb.ToString());
            }
        }
        public async Task Login(LoginDto dto)
        {
            AppUser user = await _userManager.FindByNameAsync(dto.UserNameOrEmail);
            user ??= await _userManager.FindByEmailAsync(dto.UserNameOrEmail) 
                    ?? throw new Exception("Username, Email or Password is incorrect!");
            if (!await _userManager.CheckPasswordAsync(user, dto.Password)) 
                throw new Exception("Username, Email or Password is incorrect!");
        }

        public async Task<IEnumerable<AppUserGetItemDto>> GetAllUsers(
            int? page = null,
            int? limit = null,
            bool isTracking = false,
            bool showDeleted = false,
            params string[] includes)
        {
            IEnumerable<AppUser> users = await _userManager.Users.ToListAsync();
            return _mapper.Map<IEnumerable<AppUserGetItemDto>>(users);
        }

        public async Task<AppUserGetDto> GetUserByIdAsync(
             string id,
             bool isTracking = false,
             bool showDeleted = false,
             params string[] includes)
        {
            AppUser user = await _userManager.FindByIdAsync(id)
                ?? throw new Exception("User wasnt found!");
            AppUserGetDto dto = _mapper.Map<AppUserGetDto>(user);
            return dto;


        }
    }

}
