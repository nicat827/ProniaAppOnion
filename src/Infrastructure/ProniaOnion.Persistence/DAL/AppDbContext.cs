﻿using Microsoft.EntityFrameworkCore;
using ProniaOnion.Domain.Entities;
using ProniaOnion.Domain.Entities.Common;
using System.Reflection;


namespace ProniaOnion.Persistence.DAL
{
    internal class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
        {
            
        }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Color> Colors { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<ProductTag> ProductTags { get; set; }
        public DbSet<ProductColor> ProductColors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            
            base.OnModelCreating(modelBuilder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entities = ChangeTracker.Entries<BaseEntity>();
            foreach (var data in entities)
            {
                switch (data.State)
                {
                    case EntityState.Added:
                        data.Entity.CreatedAt = DateTime.Now;
                        break;
                    case EntityState.Modified:
                        data.Entity.UpdatedAt = DateTime.Now;
                        break;

                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}