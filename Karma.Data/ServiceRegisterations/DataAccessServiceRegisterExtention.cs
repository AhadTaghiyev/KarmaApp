using System;
using Karma.Core.Entities;
using Karma.Core.Repositories;
using Karma.Data.Contexts;
using Karma.Data.Repositories;
using Microsoft.AspNetCore.Identity;
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
            services.AddScoped<IBlogRepository,BlogRepository>();
            services.AddScoped<IProductRepository,ProductRepository>();
            services.AddScoped<IProductImageRepository,ProductImageRepository>();
            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddScoped<IBasketItemRepository, BasketItemRepository>();
            services.AddScoped<IContactRepository, ContactRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddIdentity<AppUser, IdentityRole>(opt =>
            {
                opt.User.RequireUniqueEmail = true;
                opt.Lockout.MaxFailedAccessAttempts = 5;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
                opt.SignIn.RequireConfirmedEmail = true;
            })
                .AddEntityFrameworkStores<KarmaDbContext>()
                .AddDefaultTokenProviders();

        }
    }
}

