using Karma.Core.DTOS;
using Karma.Core.Entities;
using Karma.Service.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Karma.App.areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class BrandController : Controller
    {
        readonly IBrandService _brandService;

        public BrandController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _brandService.GetAllAsync());
        }
        public async Task<IActionResult> Create()
        {
            return View();  
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BrandPostDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            await _brandService.CreateAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Remove(int id)
        {
            await _brandService.RemoveAsync(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            return View(await _brandService.GetAsync(id));
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id,BrandPostDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            await _brandService.UpdateAsync(id, dto);
            return RedirectToAction(nameof(Index));
        }

    }
}
