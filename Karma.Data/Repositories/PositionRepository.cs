using System;
using Karma.Core.Entities;
using Karma.Core.Repositories;
using Karma.Data.Contexts;

namespace Karma.Data.Repositories
{
	public class PositionRepository : Repository<Position>, IPositionRepository
    {
        public PositionRepository(KarmaDbContext context) : base(context)
        {
        }
    }
}

