using System;
using Karma.Core.Entities.BaseEntities;

namespace Karma.Core.Entities
{
	public class Basket:BaseEntity
	{
		public string AppUserId { get; set; } = null!;
		public AppUser AppUser { get; set; } = null!;
		public List<BasketItem> BasketItems { get; set; } = null!;
    }

	public class BasketItem: BaseEntity
    {
		public int ProductId { get; set; }
		public Product Product { get; set; } = null!;
		public int BasketId { get; set; }
		public Basket Basket { get; set; } = null!;
		public int Count { get; set; }
	}
}

