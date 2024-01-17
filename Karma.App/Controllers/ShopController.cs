using Karma.App.ViewModels;
using Karma.Core.DTOS;
using Karma.Core.DTOS.Orders;
using Karma.Core.Entities;
using Karma.Data.Contexts;
using Karma.Service.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Karma.App.Controllers
{
    public class ShopController : Controller
    {
        readonly IColorService _colorService;
        readonly ICategoryService _categoryService;
        readonly IBrandService _brandService;
        readonly IProductService _productService;
        readonly IBasketService _basketService;
        readonly IOrderService _orderService;

        public ShopController(ICategoryService categoryService, IBrandService brandService, IColorService colorService, IProductService productService, IBasketService basketService, IOrderService orderService)
        {

            _categoryService = categoryService;
            _brandService = brandService;
            _colorService = colorService;
            _productService = productService;
            _basketService = basketService;
            _orderService = orderService;
        }
        public async Task<IActionResult> Index()
        {
            ShoopViewModel shoopViewModel = new();
            shoopViewModel.categories =await _categoryService.GetAllAsync();
            shoopViewModel.brands = await _brandService.GetAllAsync();
            shoopViewModel.colorGetDtos = await _colorService.GetAllAsync();
            shoopViewModel.ProductGetDtos = await _productService.GetAllAsync();
            return View(shoopViewModel);
        }

        public async Task<IActionResult>Detail(int id)
        {
            return View(await _productService.GetAsync(id));
        }


        public async Task<IActionResult> AddBasket(int id,int? count=1)
        {
            await _basketService.AddBasket(id,count);
            return Json(new {status=200});
        }

        public async Task<IActionResult> Basket()
        {
            return View(await _basketService.GetBasket()); ;
        }

        public async Task<IActionResult> IncreaseCount(int id)
        {
            await _basketService.AddBasket(id,null);
            return RedirectToAction(nameof(Basket));
        }
        public async Task<IActionResult> DecreaseCount(int id)
        {
            await _basketService.DecreaseCount(id);
            return RedirectToAction(nameof(Basket));
        }

        [Authorize]
        public async Task<IActionResult> Checkout()
        {
            ViewBag.Baskets = await _basketService.GetBasket();
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Checkout(OrderPostDto dto)
        {
            await _orderService.CreateAsync(dto);
            return RedirectToAction(nameof(Index));
        }

       
    }
}

