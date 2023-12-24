using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProniaOnion.Application.Dtos
{
    public record ProductGetItemDto(
        int Id,
        string Name,
        decimal Price);
   
}
