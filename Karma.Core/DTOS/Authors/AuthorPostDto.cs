using System;
using Karma.Core.Entities;

namespace Karma.Core.DTOS
{
	public record AuthorPostDto
	{
        public string FullName { get; set; } = null!;
        public string Info { get; set; } = null!;
        public int PositionId { get; set; }
        public List<string> Icons { get; set; }
        public List<string> Urls { get; set; }
      
    }
}

