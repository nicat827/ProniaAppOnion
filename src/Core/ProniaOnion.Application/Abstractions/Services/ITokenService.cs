using ProniaOnion.Application.Dtos.Token;
using ProniaOnion.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProniaOnion.Application.Abstractions.Services
{
    public interface ITokenService
    {
        Task<ResponseTokenDto> GenerateTokensAsync(AppUser user, int minutes);
    }
}
