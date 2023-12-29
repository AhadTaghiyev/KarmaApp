using System;
using Karma.Core.Entities;
using Karma.Core.Repositories;
using Karma.Data.Contexts;

namespace Karma.Data.Repositories
{
	public class BlogRepository : Repository<Blog>,IBlogRepository
    {
        public BlogRepository(KarmaDbContext context) : base(context)
        {

        }

    }
}

