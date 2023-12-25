using System;
using Karma.Core.Entities;

namespace Karma.Core.DTOS
{
	public class BlogPostDto
	{
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Image { get; set; } = null!;
        public int AuthorId { get; set; }
        public List<int> TagsIds { get; set; }= null!;
    }
}

