using Microsoft.EntityFrameworkCore;
using ProniaOnion.Domain.Entities;
using ProniaOnion.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProniaOnion.Persistence.Common
{
    internal static class GlobalQueryFilterHandler
    {
        public static void ApplyFilter<TEntity>(this ModelBuilder modelBuilder) where TEntity : BaseEntity, new()
        {
            modelBuilder.Entity<TEntity>().HasQueryFilter(e => e.IsDeleted == false);

        }
        public static void ApplyQueryFilters(this ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyFilter<Color>();
            modelBuilder.ApplyFilter<Category>();
            modelBuilder.ApplyFilter<Product>();
            modelBuilder.ApplyFilter<Tag>();
            modelBuilder.ApplyFilter<ProductColor>();
        }
    }
}
