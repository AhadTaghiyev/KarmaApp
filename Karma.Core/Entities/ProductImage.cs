using System;
using Karma.Core.Entities.BaseEntities;

namespace Karma.Core.Entities
{
	public class ProductImage:BaseEntity
	{
		public int ProductId { get; set; }
		public Product Product { get; set; } = null!;
		public bool IsMain { get; set; }
		public string Image { get; set; } = null!;
    }
}

