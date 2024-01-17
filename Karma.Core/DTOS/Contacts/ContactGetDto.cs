using System;
namespace Karma.Core.DTOS
{
	public class ContactGetDto
	{
        public int Id { get; set; }
        public string Subject { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Text { get; set; } = null!;
    }
}

