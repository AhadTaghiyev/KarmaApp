using System.Diagnostics;
using Karma.Core.DTOS;
using Karma.Core.Entities;
using Karma.Data.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Karma.App.Controllers;

public class BlogController : Controller
{
    readonly KarmaDbContext _context;

    public BlogController(KarmaDbContext context)
    {
        _context = context;
    }
    public async Task<IActionResult> Index()
    {
        IEnumerable<BlogGetDto> Blogs =await _context.Blogs.Where(x => x.IsDeleted == false)
            .Include(x => x.TagBlogs)
            .ThenInclude(x => x.Tag)
            .Include(x => x.Author)
            .AsNoTrackingWithIdentityResolution()
            .Select(blog => new BlogGetDto
            {
                Description = blog.Description,
                Image = blog.Image,
                Title = blog.Title,
                ViewCount = blog.ViewCount,
                AuthorGetDto = new AuthorGetDto { FullName = blog.Author.FullName },
                Date=blog.CreatedAt,
                Id=blog.Id,
                tags = blog.TagBlogs.Select(tagBlog => new TagGetDto { Name = tagBlog.Tag.Name })
            }).ToListAsync();

        ViewBag.Tags = _context.Tags.Where(x => !x.IsDeleted)
            .Include(x => x.TagBlogs)
            .ThenInclude(x=>x.Blog)
            .Select(tag => new {Name=tag.Name,Count=tag.TagBlogs.Where(x=>!x.Blog.IsDeleted).Count()}).AsNoTrackingWithIdentityResolution();
            

            
        return View(Blogs);
    }
    public async Task<IActionResult> Detail(int id)
    {
        var query = _context.Blogs.Where(x => !x.IsDeleted && x.Id == id)
            .Include(x => x.TagBlogs)
            .ThenInclude(x => x.Tag)
            .Include(x => x.Author)
            .ThenInclude(x=>x.Position)
            .Include(x=>x.Author)
            .ThenInclude(x=>x.SocialNetworks)
            .AsNoTrackingWithIdentityResolution();

        if (query.Count() == 0)
        {
            return NotFound();
        }
        await IncreaseCount(id);






        BlogGetDto? blogGetDto = await query.Select(blog => new BlogGetDto
        {
            Date = blog.CreatedAt,
            Description = blog.Description,
            Image = blog.Image,
            Title = blog.Title,
            ViewCount = blog.ViewCount,
            AuthorGetDto = new AuthorGetDto
            {
                FullName = blog.Author.FullName,
                Info = blog.Author.Info,
                position=new PositionGetDto { Name=blog.Author.Position.Name},
                Icons=blog.Author.SocialNetworks.Select(x=>x.Icon).ToList(),
                Urls = blog.Author.SocialNetworks.Select(x => x.Url).ToList()
            },
            tags = blog.TagBlogs.Select(x => new TagGetDto { Name = x.Tag.Name })
        }).FirstOrDefaultAsync();

        //if (blogGetDto == null)
        //{
        //    return NotFound();
        //}
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

