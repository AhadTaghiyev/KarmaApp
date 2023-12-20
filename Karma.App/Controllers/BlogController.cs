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

    public async Task<IActionResult> Create()
    {
        ViewBag.Authors =await _context.Authors.Where(x => !x.IsDeleted).AsNoTracking().ToListAsync();
        ViewBag.Tags =await _context.Tags.Where(x => !x.IsDeleted).AsNoTracking().ToListAsync();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(BlogPostDto dto)
    {

        if (!ModelState.IsValid)
        {
            ViewBag.Authors = await _context.Authors.Where(x => !x.IsDeleted).AsNoTracking().ToListAsync();
            ViewBag.Tags = await _context.Tags.Where(x => !x.IsDeleted).AsNoTracking().ToListAsync();
            return View();
        }

        if(!await _context.Authors.AnyAsync(x => x.Id == dto.AuthorId))
        {
            ModelState.AddModelError("AuthorId","Huqularimiz qorunur");
            ViewBag.Authors = await _context.Authors.Where(x => !x.IsDeleted).AsNoTracking().ToListAsync();
            ViewBag.Tags = await _context.Tags.Where(x => !x.IsDeleted).AsNoTracking().ToListAsync();
            return View();
        }

        Blog blog = new Blog
        {
            Image = dto.Image,
            Description = dto.Description,
            Title = dto.Title,
            CreatedAt = DateTime.Now,
            AuthorId=dto.AuthorId,
           
        };
        await _context.Blogs.AddAsync(blog);
        foreach (var item in dto.TagsIds)
        {
            TagBlog tagBlog = new TagBlog
            {
                CreatedAt = DateTime.Now,
                Blog = blog,
                TagId = item
            };
            //blog.TagBlogs.Add(tagBlog);
           await _context.TagBlogs.AddAsync(tagBlog);
        }
     
       await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
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

