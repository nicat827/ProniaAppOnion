﻿using ProniaOnion.Application.Abstractions.Repositories;
using ProniaOnion.Domain.Entities;
using ProniaOnion.Persistence.DAL;
using ProniaOnion.Persistence.Implementations.Repositories.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProniaOnion.Persistence.Implementations.Repositories
{
    internal class ProductTagRepository : GenericRepository<ProductTag>, IProductTagRepository 
    {
        public ProductTagRepository(AppDbContext context) : base(context)
        {
        }
    }
}
