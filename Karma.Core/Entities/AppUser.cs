using System;
using Microsoft.AspNetCore.Identity;

namespace Karma.Core.Entities
{
	public class AppUser:IdentityUser
	{
		public string FullName { get; set; } = null!;
        public List<Basket> BasketItems { get; set; } = null!;
        public List<Order> Orders { get; set; } = null!;
    }
}

