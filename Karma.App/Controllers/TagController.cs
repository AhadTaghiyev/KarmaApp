using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Karma.Core.DTOS;
using Karma.Core.Entities;
using Karma.Data.Contexts;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Karma.App.Controllers
{
    public class TagController : Controller
    {
       readonly KarmaDbContext _context;

        public TagController(KarmaDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            IEnumerable<TagGetDto> TagGetDtos = _context.Tags
                .Where(x=>!x.IsDeleted).Select(x=>new TagGetDto { Name=x.Name}).AsEnumerable();
            return Json(TagGetDtos);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(TagPostDto Tag)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if(_context.Tags.Any(x => x.Name.Trim().ToLower() == Tag.Name.Trim().ToLower()))
            {
                ModelState.AddModelError("",$"{Tag.Name}----Tag already exsist");
                return View();
            }

            Tag newTag = new Tag();
            newTag.Name = Tag.Name;

           await _context.Tags.AddAsync(newTag);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}

