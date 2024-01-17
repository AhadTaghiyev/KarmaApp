using System;
using Karma.Core.Entities.BaseEntities;

namespace Karma.Core.Entities
{
	public class Contact:BaseEntity
	{
		public string Subject { get; set; } = null!;
		public string Email { get; set; } = null!;
		public string Name { get; set; } = null!;
		public string Text { get; set; } = null!;
    }
}

