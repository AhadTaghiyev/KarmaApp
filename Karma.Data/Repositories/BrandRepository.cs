using System;
using Karma.Core.Entities;
using Karma.Core.Repositories;
using Karma.Data.Contexts;

namespace Karma.Data.Repositories
{
	public class BrandRepository : Repository<Brand>, IBrandRepository
    {
        public BrandRepository(KarmaDbContext context) : base(context)
        {

        }

    }
}

