using System;
using Karma.Core.Entities;

namespace Karma.Core.Repositories
{
	public interface IBasketRepository: IRepository<Basket>
    {
		
	}
    public interface IBasketItemRepository : IRepository<BasketItem>
    {

    }


    public interface IOrderRepository : IRepository<Order>
    {

    }
    public interface IOrderItemRepository : IRepository<OrderItem>
    {

    }
}

