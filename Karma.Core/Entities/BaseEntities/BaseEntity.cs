using System;
namespace Karma.Core.Entities.BaseEntities
{
	public class BaseEntity
	{
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

