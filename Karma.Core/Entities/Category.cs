using System;
using System.ComponentModel.DataAnnotations;
using Karma.Core.Entities.BaseEntities;

namespace Karma.Core.Entities
{
	public class Category: BaseEntity
    {
        [StringLength(30)]
        public string Name { get; set; } = null!;
        public List<Product> Products { get; set; } = null!;
    }
}

