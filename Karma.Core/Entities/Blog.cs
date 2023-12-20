using System;
using System.ComponentModel.DataAnnotations.Schema;
using Karma.Core.Entities.BaseEntities;

namespace Karma.Core.Entities
{
	public class Blog:BaseEntity
	{
		public string Title { get; set; } = null!;
		public string Description { get; set; } = null!;
		public string Image { get; set; } = null!;
		public int ViewCount { get; set; }
		public int AuthorId { get; set; }
		public Author Author { get; set; } = null!;
        public List<TagBlog> TagBlogs { get; set; }
		//[NotMapped]
  //      public List<int> TagsIds { get; set; }
    }
}

