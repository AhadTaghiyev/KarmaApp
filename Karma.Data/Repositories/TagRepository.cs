using System;
using Karma.Core.Entities;
using Karma.Core.Repositories;
using Karma.Data.Contexts;

namespace Karma.Data.Repositories
{
	public class TagRepository : Repository<Tag>, ITagRepository
    {
        public TagRepository(KarmaDbContext context) : base(context)
        {

        }

    }
}

