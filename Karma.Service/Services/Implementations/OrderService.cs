using System;
using AutoMapper;
using Karma.Core.DTOS;
using Karma.Core.DTOS.Orders;
using Karma.Core.Entities;
using Karma.Core.Enums;
using Karma.Core.Repositories;
using Karma.Service.Helpers;
using Karma.Service.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Karma.Service.Services.Implementations
{
    public class OrderService : IOrderService
    {
        readonly IOrderRepository _orderRepository;
        readonly IHttpContextAccessor _http;
        readonly UserManager<AppUser> _userManager;
        readonly IBasketRepository _basketRepository;
        readonly IMapper _mapper;

        public OrderService(IOrderRepository orderRepository, IHttpContextAccessor http, UserManager<AppUser> userManager, IBasketRepository basketRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _http = http;
            _userManager = userManager;
            _basketRepository = basketRepository;
            _mapper = mapper;
        }

        public async Task Accept(int id)
        {
            Order order = await _orderRepository.GetAsync(x=>x.Id==id);
            order.Status = (int)OrderStatus.Accept;
            await _orderRepository.UpdateAsync(order);
            await _orderRepository.SaveChangesAsync();
        }

        public async Task Compleet(int id)
        {
            Order order = await _orderRepository.GetAsync(x => x.Id == id);
            order.Status = (int)OrderStatus.Coompleted;
            await _orderRepository.UpdateAsync(order);
            await _orderRepository.SaveChangesAsync();
        }

        public async Task CreateAsync(OrderPostDto dto)
        {
            string userName = _http.HttpContext.User.Identity.Name;

            AppUser appUser = await _userManager.FindByNameAsync(userName);
            Order order = new Order
            {
                AppUser = appUser,
                Status = 0,
                OrderItems=new List<OrderItem>()
            };
            var baskets = await _basketRepository.GetQuery(x => x.IsDeleted == false)
                .Include(x => x.BasketItems).FirstOrDefaultAsync();

            foreach (var item in baskets.BasketItems.Where(x=>x.IsDeleted==false))
            {
                order.OrderItems.Add(new OrderItem
                {
                    Count=item.Count,
                    Order=order,
                    ProductId=item.ProductId,
                });
            }
            baskets.IsDeleted = true;
           await _basketRepository.UpdateAsync(baskets);
            await _orderRepository.AddAsync(order);
            await _orderRepository.SaveChangesAsync();
            Helper.SendMessageToTelegram($"Ner Order Id:{order.Id}");
        }

        public async Task<OrderGetDto> Get(int id)
        {
            var query = await _orderRepository.GetQuery(x => x.IsDeleted == false && x.Id == id)
                .Include(x => x.AppUser)
                .Include(x => x.OrderItems)
                .ThenInclude(x => x.Product)
                .FirstOrDefaultAsync();

            OrderGetDto orderGetDto = new OrderGetDto
            {
                AppUser = query.AppUser,
                Status = query.Status,
                Id = query.Id,
                OrderItems = query.OrderItems.Select(x => new OrderItemGetDto
                {
                    Count = x.Count,
                    Product = new ProductGetDto { Name = x.Product.Name, Price = x.Product.Price }
                }).ToList()
            };
            return orderGetDto;
        }

        public async Task<IEnumerable<OrderGetDto>> GetAll()
        {
            var query = _orderRepository.GetQuery(x => x.IsDeleted == false);
            return _mapper.Map<IEnumerable<OrderGetDto>>(query);
        }

        public async Task Reject(int id)
        {
            Order order = await _orderRepository.GetAsync(x => x.Id == id);
            order.Status = (int)OrderStatus.Reject;
            await _orderRepository.UpdateAsync(order);
            await _orderRepository.SaveChangesAsync();
        }
    }
}

