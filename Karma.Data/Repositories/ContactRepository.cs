using System;
using Karma.Core.Entities;
using Karma.Core.Repositories;
using Karma.Data.Contexts;

namespace Karma.Data.Repositories
{
	public class ContactRepository : Repository<Contact>,IContactRepository
    {
        public ContactRepository(KarmaDbContext context) : base(context)
        {

        }

    }
}

