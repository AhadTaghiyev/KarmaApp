using System;
using Karma.Core.Entities;

namespace Karma.Core.DTOS
{
	public class OrderGetDto
	{
		public int Id { get; set; }
        public string Adress { get; set; } = null!;
        public string Text { get; set; } = null!;
        public int Status { get; set; }
        public AppUser AppUser { get; set; } = null!;
        public List<OrderItemGetDto> OrderItems { get; set; } = null!;
    }


    public class OrderItemGetDto
    {
        public ProductGetDto Product { get; set; } = null!;
        public int Count { get; set; }
    }
}

