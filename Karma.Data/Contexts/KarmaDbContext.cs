using System;
using Karma.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Karma.Data.Contexts
{
	public class KarmaDbContext:DbContext
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
    }
}

