using ProniaOnion.Domain.Enums.AppUser;
using System.Text.Json.Serialization;

namespace ProniaOnion.Application.Dtos.AppUser
{
    public class RegisterDto 
    {
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;

        public Gender Gender { get; set;}
        public string Email { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;

    }
}
