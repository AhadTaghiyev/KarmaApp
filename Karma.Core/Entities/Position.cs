using System;
using System.ComponentModel;
using Karma.Core.Entities.BaseEntities;

namespace Karma.Core.Entities
{
	public class Position:BaseEntity
	{
		//[DisplayName("qaqa")]
		public string Name { get; set; } = null!;
		public List<Author> Authors { get; set; }
	}
}

