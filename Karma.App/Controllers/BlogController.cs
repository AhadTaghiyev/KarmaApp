using System.Diagnostics;
using Karma.Core.DTOS;
using Karma.Core.Entities;
using Karma.Data.Contexts;
using Karma.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Karma.App.Controllers;

public class BlogController : Controller
{
    readonly KarmaDbContext _context;
    readonly IBlogService _blogService;

    public BlogController(KarmaDbContext context, IBlogService blogService)
    {
        _context = context;
        _blogService = blogService;
    }
    public async Task<IActionResult> Index()
    {
        IEnumerable<BlogGetDto> Blogs = await _blogService.GetAllAsync();
        ViewBag.Tags = _context.Tags.Where(x => !x.IsDeleted)
            .Include(x => x.TagBlogs)
            .ThenInclude(x=>x.Blog)
            .Select(tag => new {Name=tag.Name,Count=tag.TagBlogs.Where(x=>!x.Blog.IsDeleted).Count()}).AsNoTrackingWithIdentityResolution();
        return View(Blogs);
    }
    public async Task<IActionResult> Detail(int id)
    {
        var blogGetDto = await _blogService.GetAsync(id);

      
        await IncreaseCount(id);

        ViewBag.Tags = _context.Tags.Where(x => !x.IsDeleted)
      .Include(x => x.TagBlogs)
      .ThenInclude(x => x.Blog)
      .Select(tag => new { Name = tag.Name, Count = tag.TagBlogs.Where(x => !x.Blog.IsDeleted).Count() }).AsNoTrackingWithIdentityResolution();

        return View(blogGetDto);
    }
    private async Task IncreaseCount(int id)
    {
        Blog?blog =await _context.Blogs.FindAsync(id);

        blog.ViewCount++;
      await  _context.SaveChangesAsync();
    }
}

