using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProniaOnion.Domain.Entities.Common
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set;}

        public bool IsDeleted { get; set; }

        public string CreatedBy { get; set; }

        public BaseEntity() {
            CreatedBy = "Nicat";
        }
    }
}
