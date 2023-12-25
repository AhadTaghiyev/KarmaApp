using Karma.Core.DTOS;
using Karma.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Karma.App.areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _categoryService.GetAllAsync());
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryPostDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            await _categoryService.CreateAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Remove(int id)
        {
            await _categoryService.RemoveAsync(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
                return View(await _categoryService.GetAsync(id));
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, CategoryPostDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            await _categoryService.UpdateAsync(id,dto);
            return RedirectToAction(nameof(Index));
        }

    }
}

