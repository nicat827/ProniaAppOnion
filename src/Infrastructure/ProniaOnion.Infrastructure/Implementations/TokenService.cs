using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ProniaOnion.Application.Abstractions.Services;
using ProniaOnion.Application.Dtos.Token;
using ProniaOnion.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ProniaOnion.Infrastructure.Implementations
{
    internal class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<AppUser> _userManager;
        public TokenService(IConfiguration configuration, UserManager<AppUser> userManager)
        {
            _configuration = configuration;
            _userManager = userManager;
        }
        public async Task<ResponseTokenDto> GenerateTokensAsync(AppUser user, int minutes)
        {
            ICollection<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.GivenName, user.Name),
                new Claim(ClaimTypes.Surname, user.Surname)
            };
            ICollection<string> roles = await  _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecurityKey"]));
            SigningCredentials signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            JwtSecurityToken token = new JwtSecurityToken
                (
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    notBefore: DateTime.UtcNow,
                    expires:DateTime.UtcNow.AddMinutes(minutes),
                    claims:claims,
                    signingCredentials:signingCredentials
                );
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            string refreshToken = Guid.NewGuid().ToString();
            return new ResponseTokenDto(handler.WriteToken(token), token.ValidTo, refreshToken, token.ValidTo.AddMinutes(minutes/2));
        
        }
    }
}
    