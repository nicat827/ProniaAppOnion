using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using ProniaOnion.Application.Abstractions.Services;
using ProniaOnion.Application.Dtos;
using ProniaOnion.Application.Dtos.AppUser;
using ProniaOnion.Application.Dtos.Token;
using ProniaOnion.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProniaOnion.Persistence.Implementations.Services
{
    internal class AuthService : IAuthService
    {
      
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly IEmailService _emailService;

        public AuthService(UserManager<AppUser> userManager, IMapper mapper, ITokenService tokenService, IEmailService emailService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _tokenService = tokenService;
            _emailService = emailService;
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
            var emailToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var validEmailToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(emailToken));
            string url = $"https://localhost:7029/api/users/confirm?token={validEmailToken}&email={user.Email}";
            Console.WriteLine(url);
            await _emailService.SendEmailAsync(user.Email, $"<a style='text-decoration:none;font-weight:600;' href=\"{url}\">Confirm account with email: {user.Email}</a>", "Confirm Account", true);

        }
        public async Task<ResponseTokenDto> LoginAsync(LoginDto dto)
        {
            AppUser user = await _userManager.FindByNameAsync(dto.UserNameOrEmail);
            user ??= await _userManager.FindByEmailAsync(dto.UserNameOrEmail) 
                    ?? throw new Exception("Username, Email or Password is incorrect!");
            if (!await _userManager.CheckPasswordAsync(user, dto.Password)) 
                throw new Exception("Username, Email or Password is incorrect!");
            ResponseTokenDto tokensDto =  await _tokenService.GenerateTokensAsync(user,15);
            user.RefreshToken = tokensDto.RefreshToken;
            user.RefreshTokenExpiresAt = tokensDto.RefreshTokenExpiresAt;
            await _userManager.UpdateAsync(user);
            return tokensDto;
        }

        public async Task<ResponseTokenDto> RefreshTokensAsync(string refreshToken)
        {
            AppUser user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken 
                                                                        && u.RefreshTokenExpiresAt > DateTime.UtcNow);
            if (user is null) throw new Exception("Token is not valid!");
            ResponseTokenDto tokensDto = await _tokenService.GenerateTokensAsync(user, 15);
            user.RefreshToken = tokensDto.RefreshToken;
            user.RefreshTokenExpiresAt = tokensDto.RefreshTokenExpiresAt;
            await _userManager.UpdateAsync(user);
            return tokensDto;

        }

        public async Task ConfirmEmailAsync(string token, string email)
        {
            AppUser user = await _userManager.FindByEmailAsync(email) ?? throw new Exception("User wasnt found!");
            var decodedToken = WebEncoders.Base64UrlDecode(token);
            string normalToken = Encoding.UTF8.GetString(decodedToken);
            var res = await _userManager.ConfirmEmailAsync(user, normalToken);
            if (!res.Succeeded)
            {
                StringBuilder sb = new StringBuilder();
                foreach(var item in res.Errors) {
                    sb.AppendLine(item.Description);
                }
                throw new Exception(sb.ToString());
            }



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
