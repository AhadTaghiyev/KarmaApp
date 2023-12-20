using System;
namespace Karma.Core.DTOS
{

    public record AuthorGetDto
    {
        public string FullName { get; set; } = null!;
        public string Info { get; set; } = null!;
        public int PositionId { get; set; }
        public PositionGetDto position { get; set; }
        public List<string> Icons { get; set; }
        public List<string> Urls { get; set; }
    }
}

