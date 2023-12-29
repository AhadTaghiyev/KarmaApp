using System;
using Karma.Core.Entities;
using Karma.Core.Repositories;
using Karma.Data.Contexts;

namespace Karma.Data.Repositories
{
	public class ProductRepository : Repository<Product>,IProductRepository
    {
        public ProductRepository(KarmaDbContext context) : base(context)
        {

        }

    }
}

