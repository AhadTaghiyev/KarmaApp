using System;
using Karma.Core.Entities.BaseEntities;

namespace Karma.Core.Entities
{
	public class Product:BaseEntity
	{
		public string Name { get; set; } = null!;
		public double Price { get; set; }
		public double DiscountPrice { get; set; }
		public int CategoryId { get; set; }
		public Category Category { get; set; } = null!;
        public int BrandId { get; set; }
        public Brand Brand { get; set; } = null!;
        public string Info { get; set; } = null!;
		public string Description { get; set; } = null!;
		public List<Specification> Specifications { get; set; } = null!;
        public List<ColorProduct> ColorProducts { get; set; } = null!;
        public List<ProductImage> ProductImages { get; set; } = null!;
        public List<BasketItem> BasketItems { get; set; } = null!;
        public List<OrderItem> OrderItems { get; set; } = null!;
    }
}

