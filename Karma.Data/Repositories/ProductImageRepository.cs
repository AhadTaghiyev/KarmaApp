using System;
using Karma.Core.Entities;
using Karma.Core.Repositories;
using Karma.Data.Contexts;

namespace Karma.Data.Repositories
{
	public class ProductImageRepository : Repository<ProductImage>,IProductImageRepository
    {
        public ProductImageRepository(KarmaDbContext context) : base(context)
        {

        }

    }
}

