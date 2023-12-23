using ProniaOnion.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProniaOnion.Domain.Entities
{
    public class Product:BaseNameableEntity
    {
        public decimal Price { get; set; }

        public string SKU { get; set; } = null!;
        public string? Description { get; set; }

        //relational props
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;

        public ICollection<ProductColor>? ProductColors { get; set; }

        public ICollection<ProductTag>? ProductTags { get; set; }

    }
}
