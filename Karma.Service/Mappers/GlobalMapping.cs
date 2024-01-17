using System;
using AutoMapper;
using Karma.Core.DTOS;
using Karma.Core.Entities;

namespace Karma.Service.Mappers
{
	public class GlobalMapping: Profile
    {
		public GlobalMapping()
		{
			CreateMap<Contact, ContactPostDto>().ReverseMap();
			CreateMap<Contact, ContactGetDto>().ReverseMap();
			CreateMap<Order, OrderGetDto>().ReverseMap();
			CreateMap<OrderItem, OrderItemGetDto>().ReverseMap();
			CreateMap<Product, ProductGetDto>().ReverseMap();
			CreateMap<AppUser, AppUser>().ReverseMap();
        }
	}
}

