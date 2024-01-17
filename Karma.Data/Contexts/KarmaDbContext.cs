using System;
using System.Reflection;
using Karma.Core.Entities;
using Karma.Core.Entities.BaseEntities;
using Karma.Data.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace Karma.Data.Contexts
{
	public class KarmaDbContext:IdentityDbContext<AppUser>
	{
		public KarmaDbContext(DbContextOptions<KarmaDbContext> options):base(options)
		{

		}
		public DbSet<Category> Categories { get; set; }
		public DbSet<Brand> Brands { get; set; }
		public DbSet<Color> Colors { get; set; }
		public DbSet<Tag> Tags { get; set; }
		public DbSet<TagBlog> TagBlogs { get; set; }
		public DbSet<Blog> Blogs { get; set; }
		public DbSet<Author> Authors { get; set; }
		public DbSet<Position> Positions { get; set; }
		public DbSet<SocialNetwork> SocialNetworks { get; set; }
		public DbSet<Product> Products { get; set; }
		public DbSet<ProductImage> ProductImages { get; set; }
		public DbSet<ColorProduct> ColorProducts { get; set; }
		public DbSet<Specification> Specifications { get; set; }
		public DbSet<Basket> Baskets { get; set; }
		public DbSet<BasketItem> BasketItems { get; set; }
		public DbSet<Contact> Contacts { get; set; }
		public DbSet<Order> Orders { get; set; }
		public DbSet<OrderItem> OrderItems { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = DateTime.UtcNow.AddHours(4);
                        break;
                    case EntityState.Modified:
                        entry.Entity.UpdatedAt = DateTime.UtcNow.AddHours(4);
                        break;
                }
            }
                return base.SaveChangesAsync(cancellationToken);
        }

        //protected override void OnModelCreating(ModelBuilder builder)
        //{
        //    builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        //    base.OnModelCreating(builder);
        //}

    }
}

