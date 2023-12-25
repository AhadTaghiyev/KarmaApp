using Karma.Core.Repositories;
using Karma.Data.Contexts;

namespace Karma.Data.Repositories
{
	public class ColorRepository:Repository<Karma.Core.Entities.Color>,IColorRepository
	{
		public ColorRepository(KarmaDbContext context):base(context)
		{
		}
	}
}

