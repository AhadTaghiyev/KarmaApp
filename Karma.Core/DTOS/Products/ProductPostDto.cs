using System;
using Karma.Core.Entities;
using Microsoft.AspNetCore.Http;

namespace Karma.Core.DTOS
{
	public record ProductPostDto
	{
        public string Name { get; set; } = null!;
        public double Price { get; set; }
        public double DiscountPrice { get; set; }
        public int CategoryId { get; set; }
        public int BrandId { get; set; }
        public string Info { get; set; } = null!;
        public string Description { get; set; } = null!;
        public List<int> ColorIds { get; set; } = null!;
        public List<int> Counts { get; set; } = null!;
        public List<string> SpecificationKeys { get; set; } = null!;
        public List<string> SpecificationValues { get; set; } = null!;
        public List<IFormFile>? ProductImageFile { get; set; } = null!;
        public IFormFile? MainImageFile { get; set; } = null!;
    }

}

