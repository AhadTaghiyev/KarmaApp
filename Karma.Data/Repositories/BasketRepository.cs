using System;
using Karma.Core.Entities;
using Karma.Core.Repositories;
using Karma.Data.Contexts;

namespace Karma.Data.Repositories
{
	public class BasketRepository : Repository<Basket>,IBasketRepository
    {
        public BasketRepository(KarmaDbContext context) : base(context)
        {

        }

    }

    public class BasketItemRepository : Repository<BasketItem>, IBasketItemRepository
    {
        public BasketItemRepository(KarmaDbContext context) : base(context)
        {

        }

    }




    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(KarmaDbContext context) : base(context)
        {

        }

    }

    public class OrderItemRepository : Repository<OrderItem>, IOrderItemRepository
    {
        public OrderItemRepository(KarmaDbContext context) : base(context)
        {

        }

    }
}

