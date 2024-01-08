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
    public class ColorController : Controller
    {
        readonly IColorService _colorService;

        public ColorController(IColorService ColorService)
        {
            _colorService = ColorService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _colorService.GetAllAsync());
        }
        public async Task<IActionResult> Create()
        {
            return View();  
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ColorPostDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            await _colorService.CreateAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Remove(int id)
        {
            await _colorService.RemoveAsync(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            return View(await _colorService.GetAsync(id));
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id,ColorPostDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            await _colorService.UpdateAsync(id, dto);
            return RedirectToAction(nameof(Index));
        }

    }
}
