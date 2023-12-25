using System;
using Karma.Core.Entities.BaseEntities;

namespace Karma.Core.Entities
{
	public class SocialNetwork: BaseEntity
    {
        public string Icon { get; set; } = null!;
        public string Url { get; set; } = null!;
        public int AuthorId { get; set; }
        public Author Author { get; set; } = null!;
    }
}

