using System;
namespace Karma.Core.DTOS
{
	public class CategoryGetDto
	{
		public int Id { get; set; }
		public string Name { get; set; } = null!;
		public DateTime CreatedAt { get; set; }
	}
}

