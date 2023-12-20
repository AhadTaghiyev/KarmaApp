using Karma.App.ViewModels;
using Karma.Core.DTOS;
using Karma.Core.Entities;
using Karma.Data.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Karma.App.Controllers
{
    public class ShopController : Controller
    {
        readonly KarmaDbContext _context;

        public ShopController(KarmaDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ShoopViewModel shoopViewModel = new();


            shoopViewModel.categories = _context.Categories.Where(x => !x.IsDeleted).AsNoTracking()
              .Select(x=>new CategoryGetDto { Name=x.Name})  .AsEnumerable();


            shoopViewModel.brands = _context.Brands.Where(x => !x.IsDeleted).AsNoTracking()
                 .Select(x => new BrandGetDto { Name = x.Name }).AsEnumerable();
            shoopViewModel.colorGetDtos= _context.Colors.Where(x => !x.IsDeleted).AsNoTracking()
                 .Select(x => new ColorGetDto { Name = x.Name }).AsEnumerable();


            return View(shoopViewModel);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            Category exsist = _context.Categories.FirstOrDefault(x=>x.Name.ToLower()==category.Name.ToLower());
            if (exsist != null)
            {
                ModelState.AddModelError("","Category already exsist");
                return View();
            }




            _context.Categories.Add(category);
            _context.SaveChanges();
            return RedirectToAction("Index","Shop");
        }



        public IActionResult CreateBrand()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateBrand(Brand brand)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            Brand? exsist = _context.Brands.FirstOrDefault(x => x.Name.ToLower() == brand.Name.ToLower());
            if (exsist != null)
            {
                ModelState.AddModelError("", "Brand already exsist");
                return View();
            }

            _context.Brands.Add(brand);
            _context.SaveChanges();
            return RedirectToAction("Index", "Shop");
        }

        public IActionResult CreateColor()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateColor(Color color)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            Color? exsist = _context.Colors.FirstOrDefault(x => x.Name.ToLower() == color.Name.ToLower());
            if (exsist != null)
            {
                ModelState.AddModelError("", "Color already exsist");
                return View();
            }

            _context.Colors.Add(color);
            _context.SaveChanges();
            return RedirectToAction("Index", "Shop");
        }



    }
}

