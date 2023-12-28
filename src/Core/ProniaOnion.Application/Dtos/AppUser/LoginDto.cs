using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProniaOnion.Application.Dtos.AppUser
{
    public record LoginDto(string UserNameOrEmail,string Password, bool IsPersistence);
    
}
