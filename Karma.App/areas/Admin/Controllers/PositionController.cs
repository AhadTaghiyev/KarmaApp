using Karma.Core.DTOS;
using Karma.Core.Entities;
using Karma.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Karma.App.areas.Admin.Controllers
{
    [Area("Admin")]
    public class PositionController : Controller
    {
        readonly IPositionService _positionService;

        public PositionController(IPositionService PositionService)
        {
            _positionService = PositionService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _positionService.GetAllAsync());
        }
        public async Task<IActionResult> Create()
        {
            return View();  
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PositionPostDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            await _positionService.CreateAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Remove(int id)
        {
            await _positionService.RemoveAsync(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            return View(await _positionService.GetAsync(id));
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id,PositionPostDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            await _positionService.UpdateAsync(id, dto);
            return RedirectToAction(nameof(Index));
        }

    }
}
