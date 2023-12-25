using Karma.App.ViewModels;
using Karma.Core.DTOS;
using Karma.Core.Entities;
using Karma.Data.Contexts;
using Karma.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Karma.App.Controllers
{
    public class ShopController : Controller
    {
        readonly IColorService _colorService;
        readonly ICategoryService _categoryService;
        readonly IBrandService _brandService;

        public ShopController(ICategoryService categoryService, IBrandService brandService, IColorService colorService)
        {

            _categoryService = categoryService;
            _brandService = brandService;
            _colorService = colorService;
        }
        public async Task<IActionResult> Index()
        {
            ShoopViewModel shoopViewModel = new();
            shoopViewModel.categories =await _categoryService.GetAllAsync();
            shoopViewModel.brands = await _brandService.GetAllAsync();
            shoopViewModel.colorGetDtos = await _colorService.GetAllAsync();
            return View(shoopViewModel);
        }
    }
}

