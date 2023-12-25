using System;
using Karma.Core.Entities;
using Karma.Core.Repositories;
using Karma.Data.Contexts;

namespace Karma.Data.Repositories
{
	public class AuthorRepository : Repository<Author>,IAuthorRepository
    {
        public AuthorRepository(KarmaDbContext context) : base(context)
        {

        }

    }
}

