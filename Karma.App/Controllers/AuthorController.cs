using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Karma.Core.DTOS;
using Karma.Core.Entities;
using Karma.Data.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Karma.App.Controllers
{
    public class AuthorController : Controller
    {
        readonly KarmaDbContext _context;

        public AuthorController(KarmaDbContext context)
        {
            _context = context;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return Json(_context.Authors.Include(x=>x.Position).Include(x=>x.SocialNetworks).ToList());
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Positions = _context.Positions.Where(x=>!x.IsDeleted).ToList();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(AuthorPostDto dto)
        {
            ViewBag.Positions = _context.Positions.Where(x => !x.IsDeleted).ToList();
            if (!ModelState.IsValid) 
                return View();

            if (!_context.Positions.Any(x => x.Id == dto.PositionId))
            {
                ModelState.AddModelError("", "Position invalid");

                return View();
            }
            Author author = new Author
            {
                CreatedAt = DateTime.Now,
                FullName = dto.FullName,
                Info = dto.Info,
                PositionId = dto.PositionId,
                SocialNetworks = new List<SocialNetwork>()
            };

            for (int i = 0; i < dto.Icons.Count(); i++)
            {
                SocialNetwork socialNetwork = new SocialNetwork();
                socialNetwork.Icon = dto.Icons[i];
                socialNetwork.Url = dto.Urls[i];
                //author.SocialNetworks.Add(socialNetwork);
                //socialNetwork.AuthorId = author.Id;
                socialNetwork.Author = author;
               await _context.SocialNetworks.AddAsync(socialNetwork);
            }

            
           await _context.Authors.AddAsync(author);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}

