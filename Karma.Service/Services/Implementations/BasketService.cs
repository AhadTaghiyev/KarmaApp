using System;
using Karma.Core.DTOS;
using Karma.Service.Exceptions;
using Karma.Service.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Karma.Service.Services.Implementations
{
    public class BasketService : IBasketService
    {
        readonly IProductService _productService;
        readonly IHttpContextAccessor _http;

        public BasketService(IProductService productService, IHttpContextAccessor http)
        {
            _productService = productService;
            _http = http;
        }

        public async Task AddBasket(int id, int? count)
        {
            var product = await _productService.GetAsync(id);
            if (product == null) throw new ItemNotFoundExcpetion("Product Not found");
            List<BasketDto>? basketDtos = new List<BasketDto>();

            var basketJson = _http.HttpContext.Request.Cookies["basket"];

            if (basketJson == null)
            {
                BasketDto basketDto = new BasketDto()
                {
                    Id = id,
                    Count = 1
                };
                basketDtos.Add(basketDto);
            }
            else
            {
                basketDtos = JsonConvert.DeserializeObject<List<BasketDto>>(basketJson);

                BasketDto? basketDto = basketDtos.FirstOrDefault(x => x.Id == id);

                if (basketDto == null)
                {
                    basketDto = new BasketDto()
                    {
                        Id = id,
                        Count = 1
                    };
                    basketDtos.Add(basketDto);
                }
                else
                {
                    basketDto.Count++;
                }

            }


            basketJson = JsonConvert.SerializeObject(basketDtos);
           _http.HttpContext.Response.Cookies.Append("basket", basketJson);
        }

        public async Task DecreaseCount(int id)
        {
            List<BasketDto>? basketDtos = new List<BasketDto>();
            var product = await _productService.GetAsync(id);
            if (product == null) throw new ItemNotFoundExcpetion("Product Not found");
            var basketJson = _http.HttpContext.Request.Cookies["basket"];
            if (basketJson == null)
            {
                if (product == null) throw new ItemNotFoundExcpetion("Product Not found");
            }
            else
            {
                basketDtos = JsonConvert.DeserializeObject<List<BasketDto>>(basketJson);

                BasketDto? basketDto = basketDtos.FirstOrDefault(x => x.Id == id);

                if (basketDto == null)
                {
                    throw new ItemNotFoundExcpetion("Product Not found");
                }else if (basketDto.Count == 1)
                {
                    basketDtos.Remove(basketDto);
                }
                else
                {
                    basketDto.Count--;
                }

            }
            basketJson = JsonConvert.SerializeObject(basketDtos);
            _http.HttpContext.Response.Cookies.Append("basket", basketJson);
        }

        public async Task<BasketGetDto> GetBasket()
        {
            BasketGetDto basketGetDto = new BasketGetDto();
            var basketJson = _http.HttpContext.Request.Cookies["basket"];

            if (basketJson != null)
            {
              List<BasketDto>  basketDtos = JsonConvert.DeserializeObject<List<BasketDto>>(basketJson);

                foreach (var item in basketDtos)
                {
                   var product= await _productService.GetAsync(item.Id);
                    if (product != null)
                    {
                        var basketItem = new BasketItem
                        {
                            Id = product.Id,
                            Count = item.Count,
                            Image = product.ProductImage.FirstOrDefault(x => !x.IsDeleted && x.IsMain).Image,
                            Name = product.Name,
                            Price = product.DiscountPrice != 0 && product.DiscountPrice < product.Price ? product.DiscountPrice : product.Price,
                        };
                        basketGetDto.basketItems.Add(basketItem);
                        basketGetDto.TotalPrice += basketItem.Price * basketItem.Count;
                    }
                }
            }
            return basketGetDto;
        }
    }
}

