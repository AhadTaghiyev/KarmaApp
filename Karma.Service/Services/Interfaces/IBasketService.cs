using System;
using Karma.Core.DTOS;

namespace Karma.Service.Services.Interfaces
{
	public interface IBasketService
	{
		public Task AddBasket(int id,int? count);

		public Task<BasketGetDto> GetBasket();
	}
}

