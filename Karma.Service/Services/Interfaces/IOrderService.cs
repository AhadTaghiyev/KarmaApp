using System;
using Karma.Core.DTOS;
using Karma.Core.DTOS.Orders;

namespace Karma.Service.Services.Interfaces
{
	public interface IOrderService
	{
		public Task CreateAsync(OrderPostDto dto);
		public Task<IEnumerable<OrderGetDto>> GetAll();
		public Task<OrderGetDto> Get(int id);
        public Task Accept(int id);
		public Task Reject(int id);
		public Task Compleet(int id);
    }
}

