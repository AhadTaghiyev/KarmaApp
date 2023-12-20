using System;
using Karma.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Karma.Data.ServiceRegisterations
{
	public static class DataAccessServiceRegisterExtention
    {
		public static void DataAccessServiceRegister(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddDbContext<KarmaDbContext>(opt =>
            {
                opt.UseSqlServer(configuration.GetConnectionString("Default"));
            });
        }
    }
}

