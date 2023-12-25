using System;
using Karma.Core.Repositories;
using Karma.Data.Contexts;
using Karma.Data.Repositories;
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
            services.AddScoped<IBrandRepository,BrandRepository>();
            services.AddScoped<ICategoryRepository,CategoryRepository>();
            services.AddScoped<IColorRepository,ColorRepository>();
            services.AddScoped<IPositionRepository,PositionRepository>();
            services.AddScoped<ITagRepository,TagRepository>();
            services.AddScoped<IAuthorRepository,AuthorRepository>();
        }
    }
}

