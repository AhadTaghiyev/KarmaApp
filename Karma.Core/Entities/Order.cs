using System;
using Karma.Core.Entities.BaseEntities;
using Karma.Core.Enums;

namespace Karma.Core.Entities
{
	public class Order:BaseEntity
	{
        public string AppUserId { get; set; } = null!;
        public AppUser AppUser { get; set; } = null!;
        public List<OrderItem> OrderItems { get; set; } = null!;
        public int Status { get; set; }

    }

    public class OrderItem: BaseEntity
    {
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;
        public int Count { get; set; }
    }

}

