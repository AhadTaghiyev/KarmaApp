using Karma.Service.ExternalServices.Implementations;
using Karma.Service.ExternalServices.Interfaces;
using Karma.Service.Services.Implementations;
using Karma.Service.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Karma.Data.ServiceRegisterations
{
	public static class ServiceLaierServiceRegisterExtention
    {
		public static void ServiceLayerServiceRegister(this IServiceCollection services)
        {
            services.AddScoped<ICategoryService,CategoryService>();
            services.AddScoped<IBrandService,BrandService>();
            services.AddScoped<IColorService,ColorService>();
            services.AddScoped<IPositionService,PositionService>();
            services.AddScoped<ITagService,TagService>();
            services.AddScoped<IAuthorService,AuthorService>();
            services.AddScoped<IBlogService, BlogService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProductImageService, ProductImageService>();
            services.AddScoped<IBasketService, BasketService>();
            services.AddHttpContextAccessor();
            services.AddScoped<IEmailService, EmailService>();
        }
    }
}

