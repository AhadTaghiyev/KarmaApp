using System;
using System.ComponentModel.DataAnnotations;
using Karma.Core.Entities.BaseEntities;

namespace Karma.Core.Entities
{
    public class Color : BaseEntity
    {
        [StringLength(30)]
        public string Name { get; set; } = null!;
    }
}

