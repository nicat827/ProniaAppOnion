using ProniaOnion.Application.Dtos;
using ProniaOnion.Application.Dtos.AppUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProniaOnion.Application.Abstractions.Services
{
    public interface IAuthService
    {
        Task Register(RegisterDto dto);
        Task Login(LoginDto dto);

        Task<IEnumerable<AppUserGetItemDto>> GetAllUsers(
            int? page = null,
            int? limit = null,
            bool isTracking = false,
            bool showDeleted = false,
            params string[] includes);

        Task<AppUserGetDto> GetUserByIdAsync(
        string id,
        bool isTracking = false,
        bool showDeleted = false,
        params string[] includes);
    }
}
