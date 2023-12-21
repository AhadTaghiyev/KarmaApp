using System;
namespace Karma.Core.DTOS
{
	public class BrandGetDto
	{
        public int Id { get; set; }
        public string Name { get; set; } = null!;
		public DateTime CreatedAt { get; set; }
	}
}

