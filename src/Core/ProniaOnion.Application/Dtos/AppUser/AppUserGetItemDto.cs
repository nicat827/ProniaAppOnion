using ProniaOnion.Domain.Enums.AppUser;
using System.Text.Json.Serialization;

namespace ProniaOnion.Application.Dtos.AppUser
{
    //public record AppUserGetItemDto(string Id, string Name, string Surname, Gender Gender);
    public class AppUserGetItemDto
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;

        public Gender Gender { get; set;}
    }
    


}
