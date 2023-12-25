using System;
using Karma.Core.Entities.BaseEntities;

namespace Karma.Core.Entities
{
	public class Tag : BaseEntity
    {
		public string Name { get; set; } = null!;
		public List<TagBlog> TagBlogs { get; set; }
	}
}

