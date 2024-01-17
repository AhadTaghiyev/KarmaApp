using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Karma.Core.DTOS;
using Karma.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Karma.App.Controllers
{
    public class ContactController : Controller
    {
        readonly IContactService _contactService;

        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(ContactPostDto dto)
        {
            await _contactService.CreateAsync(dto);
            return RedirectToAction(nameof(Index));
        }
    }
}

