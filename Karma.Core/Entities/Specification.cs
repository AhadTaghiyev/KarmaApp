using System;
using Karma.Core.Entities.BaseEntities;

namespace Karma.Core.Entities
{
	public class Specification:BaseEntity
	{
		public string Key { get; set; } = null!;
		public string Value { get; set; } = null!;
		public int ProductId { get; set; }
		public Product Product { get; set; } = null!;
    }
}

