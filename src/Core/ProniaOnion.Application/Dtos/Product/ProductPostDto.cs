using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProniaOnion.Application.Dtos
{
    public record ProductPostDto(
        string Name,   
        decimal Price,
        int CategoryId,
        string? Description,
        IEnumerable<int>? TagIds,
        IEnumerable<int>? ColorIds);
    
}
