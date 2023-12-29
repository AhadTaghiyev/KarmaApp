using System;
using Karma.Core.Entities.BaseEntities;

namespace Karma.Core.Entities
{
	public class ColorProduct:BaseEntity
	{
		public int ProductId { get; set; }
		public int ColorId { get; set; }
		public Product Product { get; set; } = null!;
		public Color Color { get; set; } = null!;
		public int StockCount { get; set; }
    }
}

