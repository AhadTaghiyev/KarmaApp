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
    public class PositionController : Controller
    {
       readonly KarmaDbContext _context;

        public PositionController(KarmaDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            IEnumerable<PositionGetDto> positionGetDtos = _context.Positions
                .Where(x=>!x.IsDeleted).Select(x=>new PositionGetDto { Name=x.Name}).AsEnumerable();
            return Json(positionGetDtos);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(PositionPostDto position)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if(_context.Positions.Any(x => x.Name.Trim().ToLower() == position.Name.Trim().ToLower()))
            {
                ModelState.AddModelError("",$"{position.Name}----Position already exsist");
                return View();
            }

            Position newPosition = new Position();
            newPosition.Name = position.Name;

           await _context.Positions.AddAsync(newPosition);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}

