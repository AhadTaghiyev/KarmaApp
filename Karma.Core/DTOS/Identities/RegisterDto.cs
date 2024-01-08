using System;
using System.ComponentModel.DataAnnotations;

namespace Karma.Core.DTOS
{
	public record RegisterDto
	{
		public string Username { get; set; } = null!;
		[DataType(DataType.EmailAddress,ErrorMessage ="Email is not valid")]
		public string Email { get; set; } = null!;
		public string FullName { get; set; } = null!;
		[DataType(DataType.Password)]
		public string Password { get; set; } = null!;
		[DataType(DataType.Password)]
		[Compare("Password")]
		public string ConfirmPassword { get; set; } = null!;
		[Required]
		public bool Terms { get; set; }

    }
}

