using System;
using Karma.Core.DTOS;
using x=Karma.Core.DTOS;
using Karma.Core.Entities;
using Karma.Core.Repositories;
using Karma.Service.Exceptions;
using Karma.Service.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;

namespace Karma.Service.Services.Implementations
{
    public class BasketService : IBasketService
    {
        readonly IProductService _productService;
        readonly IHttpContextAccessor _http;
        readonly IBasketRepository _basketRepository;
        readonly IBasketItemRepository _basketItemRepository;
        readonly UserManager<AppUser> _userManager;

        public BasketService(IProductService productService, IHttpContextAccessor http, IBasketRepository basketRepository, IBasketItemRepository basketItemRepository, UserManager<AppUser> userManager)
        {
            _productService = productService;
            _http = http;
            _basketRepository = basketRepository;
            _basketItemRepository = basketItemRepository;
            _userManager = userManager;
        }

        public async Task AddBasket(int id, int? count)
        {
            var product = await _productService.GetAsync(id);
            if (product == null) throw new ItemNotFoundExcpetion("Product Not found");
            if (_http.HttpContext.User.Identity.IsAuthenticated)
            {
                AppUser appUser = await _userManager.FindByNameAsync(_http.HttpContext.User.Identity.Name);

                var basket = await _basketRepository.GetAsync(x=>!x.IsDeleted&&x.AppUserId==appUser.Id, "BasketItems");

                if (basket != null)
                {

                    var basketItem = basket.BasketItems.Where(X=>!X.IsDeleted).FirstOrDefault(x => x.ProductId == id);

                    if (basketItem != null)
                    {
                        basketItem.Count++;
                        await _basketItemRepository.UpdateAsync(basketItem);
                    }
                    else
                    {
                        basketItem = new Core.Entities.BasketItem
                        {
                            Count = count??1,
                            Basket = basket,
                            ProductId = id,
                        };
                        await _basketItemRepository.AddAsync(basketItem);
                    }
                }
                else
                {
                    basket = new Basket { AppUserId = appUser.Id };
                    await _basketRepository.AddAsync(basket);
                    var basketItem = new Core.Entities.BasketItem
                    {
                        Count = 1,
                        Basket = basket,
                        ProductId = id,
                    };
                    await _basketItemRepository.AddAsync(basketItem);
                }
                await _basketRepository.SaveChangesAsync();
            }
            else
            {
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
        }

        public async Task DecreaseCount(int id)
        {
            var product = await _productService.GetAsync(id);
            if (product == null) throw new ItemNotFoundExcpetion("Product Not found");

            if (_http.HttpContext.User.Identity.IsAuthenticated)
            {
                AppUser appUser = await _userManager.FindByNameAsync(_http.HttpContext.User.Identity.Name);

                var basket = await _basketRepository.GetAsync(x => !x.IsDeleted && x.AppUserId == appUser.Id, "BasketItems");

                if (basket == null)
                {
                    if (basket == null) throw new ItemNotFoundExcpetion("basket Not found");
                }

                var basketItem = basket.BasketItems.Where(X => !X.IsDeleted).FirstOrDefault(x => x.ProductId == id);

                if (basketItem == null)
                {
                    if (basketItem == null) throw new ItemNotFoundExcpetion("basket Not found");
                }

               else if (basketItem.Count == 1)
                {
                    basketItem.IsDeleted = true;
                }
                else
                {
                    basketItem.Count--;
                }
                await _basketItemRepository.UpdateAsync(basketItem);
                await _basketItemRepository.SaveChangesAsync();
            }
            else
            {
                List<BasketDto>? basketDtos = new List<BasketDto>();
  
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
                    }
                    else if (basketDto.Count == 1)
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
        }

        public async Task<BasketGetDto> GetBasket()
        {
            BasketGetDto basketGetDto = new BasketGetDto();

            if (_http.HttpContext.User.Identity.IsAuthenticated)
            {
                AppUser appUser = await _userManager.FindByNameAsync(_http.HttpContext.User.Identity.Name);

                var basketquery =await _basketRepository.GetQuery(x => !x.IsDeleted && x.AppUserId == appUser.Id)
                    .Include(x => x.BasketItems)
                    .ThenInclude(x=>x.Product)
                    .ThenInclude(x=>x.ProductImages)
                    .FirstOrDefaultAsync();

                if (basketquery != null)
                {
                    basketGetDto.basketItems = basketquery.BasketItems.Where(X => !X.IsDeleted).Select(bs => new x.BasketItem
                    {
                        Count = bs.Count,
                        Id = bs.ProductId,
                        Image = bs.Product.ProductImages.FirstOrDefault(x => !x.IsDeleted && x.IsMain).Image,
                        Name = bs.Product.Name,
                        Price = bs.Product.DiscountPrice != 0 && bs.Product.DiscountPrice < bs.Product.Price ? bs.Product.DiscountPrice : bs.Product.Price,

                    }).ToList();
                    basketGetDto.TotalPrice = basketGetDto.basketItems.Sum(x => (x.Price*x.Count));
                }
            }
            else
            {
                var basketJson = _http.HttpContext.Request.Cookies["basket"];

                if (basketJson != null)
                {
                    List<BasketDto> basketDtos = JsonConvert.DeserializeObject<List<BasketDto>>(basketJson);

                    foreach (var item in basketDtos)
                    {
                        var product = await _productService.GetAsync(item.Id);
                        if (product != null)
                        {
                            var basketItem = new x.BasketItem
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
            }
            return basketGetDto;
        }
    }
}

