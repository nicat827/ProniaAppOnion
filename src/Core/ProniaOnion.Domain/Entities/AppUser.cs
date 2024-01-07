using Microsoft.AspNetCore.Identity;
using ProniaOnion.Domain.Enums.AppUser;
using System.Text.Json.Serialization;

namespace ProniaOnion.Domain.Entities
{
    public class AppUser: IdentityUser
    {
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;

        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiresAt { get; set; }
        public Gender Gender { get; set; }

        public bool IsActive { get; set; }

    }
}
