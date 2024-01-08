using System;
using Karma.Core.DTOS;
using Karma.Service.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Karma.App.areas.Admin.Controllers
{
	[Area("Admin")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class ProductController: Controller
    {
        readonly ICategoryService _categoryService;
        readonly IColorService _colorService;
        readonly IBrandService _brandService;
        readonly IProductService _productService;
        readonly IProductImageService _productImageService;

        public ProductController(ICategoryService categoryService, IColorService colorService, IBrandService brandService, IProductService productService, IProductImageService productImageService)
        {
            _categoryService = categoryService;
            _colorService = colorService;
            _brandService = brandService;
            _productService = productService;
            _productImageService = productImageService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _productService.GetAllAsync());
        }
        public async Task<IActionResult> Create()
		{
            ViewBag.Colors =await _colorService.GetAllAsync();
            ViewBag.Brands= await _brandService.GetAllAsync();
            ViewBag.Categories= await _categoryService.GetAllAsync();
            return View();
		}

        [HttpPost]
        public async Task<IActionResult> Create(ProductPostDto dto)
        {
            if (!ModelState.IsValid)
            {

                ViewBag.Colors = await _colorService.GetAllAsync();
                ViewBag.Brands = await _brandService.GetAllAsync();
                ViewBag.Categories = await _categoryService.GetAllAsync();
                return View(dto);
            };
            var res = await _productService.CreateAsync(dto);
            if (res.StatusCode!=200)
            {
                ViewBag.Colors = await _colorService.GetAllAsync();
                ViewBag.Brands = await _brandService.GetAllAsync();
                ViewBag.Categories = await _categoryService.GetAllAsync();
                ModelState.AddModelError("", res.Message);
                return View(dto);
            };
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Remove(int id)
        {
           await _productService.RemoveAsync(id);
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Update(int id)
        {
            ViewBag.Colors = await _colorService.GetAllAsync();
            ViewBag.Brands = await _brandService.GetAllAsync();
            ViewBag.Categories = await _categoryService.GetAllAsync();
            var product = await _productService.GetAsync(id);
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id,ProductPostDto dto)
        {
            if (!ModelState.IsValid)
            {

                ViewBag.Colors = await _colorService.GetAllAsync();
                ViewBag.Brands = await _brandService.GetAllAsync();
                ViewBag.Categories = await _categoryService.GetAllAsync();
                var product = await _productService.GetAsync(id);
                return View(product);
            };
            var res = await _productService.UpdateAsync(id,dto);
            if (res.StatusCode != 200)
            {
                ViewBag.Colors = await _colorService.GetAllAsync();
                ViewBag.Brands = await _brandService.GetAllAsync();
                ViewBag.Categories = await _categoryService.GetAllAsync();
                var product = await _productService.GetAsync(id);
                ModelState.AddModelError("", res.Message);
                return View(product);
            };
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> RemoveImage(int id)
        {
            await _productImageService.RemoveAsync(id);
            return Json(new {status=200});
        }
    }
}

