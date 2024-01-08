using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Karma.Core.DTOS;
using Karma.Service.Services.Implementations;
using Karma.Service.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Karma.App.areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class AuthorController : Controller
    {
        readonly IAuthorService _authorService;
        readonly IPositionService _positionService;
   

        public AuthorController(IAuthorService authorService, IPositionService positionService)
        {
            _authorService = authorService;
            _positionService = positionService;
        }
      
        public async Task<IActionResult> Index(int page = 1)
        {
         
            return View(await _authorService.GetAllAsync(page));
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Positions = await _positionService.GetAllAsync();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AuthorPostDto dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Positions = await _positionService.GetAllAsync();
                return View();
            }
           var response= await _authorService.CreateAsync(dto);

            if (response.StatusCode != 200)
            {
                ViewBag.Positions = await _positionService.GetAllAsync();
                ModelState.AddModelError("",response.Message);
                return View();
            }
            
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
          
            ViewBag.Positions = await _positionService.GetAllAsync();
            return View(await _authorService.GetAsync(id));
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, AuthorPostDto dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Positions = await _positionService.GetAllAsync();
                return View(await _authorService.GetAsync(id));
            }
            var response = await _authorService.UpdateAsync(id,dto);

            if (response.StatusCode != 200)
            {
                ViewBag.Positions = await _positionService.GetAllAsync();
                ModelState.AddModelError("", response.Message);
                return View(await _authorService.GetAsync(id));
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Remove(int id)
        {
            await _authorService.RemoveAsync(id);
            return RedirectToAction(nameof(Index));
        }


    }
}

