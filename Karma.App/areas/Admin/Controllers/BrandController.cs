using Karma.Core.DTOS;
using Karma.Core.Entities;
using Karma.Data.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Karma.App.areas.Admin.Controllers
{
    [Area("Admin")]
    public class BrandController : Controller
    {
        readonly KarmaDbContext _context;

        public BrandController(KarmaDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<BrandGetDto> brands = await _context.Brands.Where(x => !x.IsDeleted)
                .AsNoTrackingWithIdentityResolution().Select(x => new BrandGetDto { Name = x.Name, Id = x.Id, CreatedAt = x.CreatedAt })
                .ToListAsync();
            return View(brands);
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
            Brand brand = new Brand();
            brand.CreatedAt=DateTime.Now;
            brand.Name=dto.Name;
            await _context.Brands.AddAsync(brand);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Remove(int id)
        {
            Brand? brand = await _context.Brands.Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();

            if(brand == null)
            {
                return NotFound();
            }

            brand.IsDeleted = true;
           await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            Brand? brand = await _context.Brands.Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();

            if (brand == null)
            {
                return NotFound();
            }
            return View(brand);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id,BrandPostDto dto)
        {
           
            Brand? brand = await _context.Brands.Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();

            if (brand == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(brand);
            }

            brand.Name = dto.Name;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
