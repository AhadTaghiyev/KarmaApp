using System;
using Karma.Core.Entities.BaseEntities;

namespace Karma.Core.Entities
{
	public class Author: BaseEntity
    {
		public string FullName { get; set; } = null!;
		public string Info { get; set; } = null!;
		public Position Position { get; set; } = null!;
		public int PositionId { get; set; }
		public List<SocialNetwork> SocialNetworks { get; set; }
		public List<Blog> Blogs { get; set; }
    }
}

