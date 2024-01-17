using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Karma.Core.DTOS;
using Karma.Service.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Karma.App.areas.Admin.Controllers
{
    [Authorize(Roles ="Admin,SuperAdmin")]
    [Area("Admin")]
    public class ContactController : Controller
    {
        readonly IContactService _contactService;

        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _contactService.GetAllAsync());
        }
        public async Task<IActionResult> Remove(int id)
        {
            await _contactService.RemoveAsync(id);
            return RedirectToAction(nameof(Index));
        }

    }
}

