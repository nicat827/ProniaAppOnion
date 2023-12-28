using ProniaOnion.Domain.Enums.AppUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProniaOnion.Application.Dtos.AppUser
{
    public record AppUserGetDto(string Id, string Name, string Surname, Gender Gender, string Email, string UserName);
   
}
