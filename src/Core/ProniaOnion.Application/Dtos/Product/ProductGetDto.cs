using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProniaOnion.Application.Dtos
{
    public class ProductGetDto 
    {  
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public  CategoryGetDto Category { get; set; } = null!;
        public decimal Price {get; set;}
        public string SKU { get; set; } = null!;
        public string? Description{get; set;}  = null;
        public ICollection<TagGetItemDto> Tags {get; set;} = new List<TagGetItemDto>();
        public ICollection<string> ColorNames { get; set; } = new List<string>();
    } 
    
}
