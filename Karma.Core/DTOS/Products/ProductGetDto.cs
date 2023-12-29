using System;
using Karma.Core.Entities;

namespace Karma.Core.DTOS
{
	public record ProductGetDto
	{
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public double Price { get; set; }
        public double DiscountPrice { get; set; }
        public CategoryGetDto Category { get; set; } = null!;
        public int BrandId { get; set; }
        public int CategoryId { get; set; }
        public BrandGetDto Brand { get; set; } = null!;
        public string Info { get; set; } = null!;
        public string Description { get; set; } = null!;
        public List<Specification> Specifications { get; set; } = null!;
        public List<ColorProduct> ColorProducts { get; set; } = null!;
        public List<ProductImage> ProductImage { get; set; } = null!;
    }
}

