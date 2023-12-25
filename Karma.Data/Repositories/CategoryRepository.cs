using System;
using Karma.Core.Entities;
using Karma.Core.Repositories;
using Karma.Data.Contexts;

namespace Karma.Data.Repositories
{
	public class CategoryRepository:Repository<Category>,ICategoryRepository
	{
		public CategoryRepository(KarmaDbContext context):base(context)
		{

		}
	}
}

