using System;
using Karma.Core.DTOS;
using Karma.Core.Entities;
using Karma.Core.Repositories;
using Karma.Data.Repositories;
using Karma.Service.Exceptions;
using Karma.Service.Extentions;
using Karma.Service.Responses;
using Karma.Service.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace Karma.Service.Services.Implementations
{
    public class BlogService : IBlogService
    {
        readonly IWebHostEnvironment _env;
        readonly IBlogRepository _blogRepository;

        public BlogService(IWebHostEnvironment env, IBlogRepository blogRepository)
        {
            _env = env;
            _blogRepository = blogRepository;
        }

        public async Task<CommonResponse> CreateAsync(BlogPostDto dto)
        {
            CommonResponse commonResponse = new CommonResponse();
            commonResponse.StatusCode = 200;

            Blog blog = new Blog
            {
                AuthorId = dto.AuthorId,
                Description = dto.Description,
                Title = dto.Title,
                TagBlogs = new List<TagBlog>()
            };

            if (dto.ImageFile == null)
            {
                commonResponse.StatusCode = 400;
                commonResponse.Message = "The field image is required";
                return commonResponse;
            }

            if (!dto.ImageFile.IsImage())
            {
                commonResponse.StatusCode = 400;
                commonResponse.Message = "Image is not valid";
                return commonResponse;
            }

            if (dto.ImageFile.IsSizeOk(1))
            {
                commonResponse.StatusCode = 400;
                commonResponse.Message = "Image  size is not valid";
                return commonResponse;
            }

            blog.Image = dto.ImageFile.SaveFile(_env.WebRootPath, "img/blog");

            foreach (var item in dto.TagsIds)
            {
                blog.TagBlogs.Add(new TagBlog
                {
                    Blog=blog,
                    TagId= item,
                });
            }

            await _blogRepository.AddAsync(blog);
            await _blogRepository.SaveChangesAsync();
            return commonResponse;
        }

        public async Task<IEnumerable<BlogGetDto>> GetAllAsync()
        {
            IEnumerable<BlogGetDto> Authors = await _blogRepository.GetQuery(x => !x.IsDeleted)
              .AsNoTrackingWithIdentityResolution()
              .Include(x => x.TagBlogs)
              .ThenInclude(x=>x.Tag)
              .Include(x=>x.Author)
              .ThenInclude(x=>x.Position)
              .Select(x =>
              new BlogGetDto
              {
                  Title = x.Title,
                  Id = x.Id,
                   Description=x.Description,
                   tags=x.TagBlogs.Select(y=>new TagGetDto { Name=y.Tag.Name}),
                  Image = x.Image,
                  ViewCount=x.ViewCount,
                  Date=x.CreatedAt,
                  AuthorGetDto=new AuthorGetDto { FullName=x.Author.FullName,Image=x.Author.Image}
              })
              .ToListAsync();
            return Authors;
        }

        public async  Task<BlogGetDto> GetAsync(int id)
        {
            Blog? blog = await _blogRepository.GetAsync(x => !x.IsDeleted && x.Id == id, "TagBlogs.Tag", "Author.Position", "Author.SocialNetworks");
   
            if (blog == null)
            {
                throw new ItemNotFoundExcpetion("Blog Not Found");
            }

            BlogGetDto BlogGetDto = new BlogGetDto
            {
                Id = blog.Id,
                Date=blog.CreatedAt,
                Description=blog.Description,
                Image=blog.Image,
                tags=blog.TagBlogs.Select(x=>new TagGetDto {Name=x.Tag.Name,Id=x.Tag.Id}),
                Title=blog.Title,
                ViewCount=blog.ViewCount,
                AuthorGetDto=new AuthorGetDto { FullName=blog.Author.FullName,Id=blog.Author.Id,
                position=new PositionGetDto { Name=blog.Author.Position.Name,
                },
                Icons=blog.Author.SocialNetworks.Select(x=>x.Icon).ToList(),
                Urls = blog.Author.SocialNetworks.Select(x => x.Url).ToList()
                },
                
            };
            return BlogGetDto;
        }

        public async Task RemoveAsync(int id)
        {
            Blog? blog = await _blogRepository.GetAsync(x => !x.IsDeleted && x.Id == id, "TagBlogs.Tag", "Author.Position");

            if (blog == null)
            {
                throw new ItemNotFoundExcpetion("Blog Not Found");
            }
            blog.IsDeleted = true;
            await _blogRepository.UpdateAsync(blog);
            await _blogRepository.SaveChangesAsync();
        }

        public async Task<CommonResponse> UpdateAsync(int id, BlogPostDto dto)
        {
            CommonResponse commonResponse = new CommonResponse();
            commonResponse.StatusCode = 200;
            Blog? blog = await _blogRepository.GetAsync(x => !x.IsDeleted && x.Id == id, "TagBlogs.Tag", "Author.Position");

            if (blog == null)
            {
                throw new ItemNotFoundExcpetion("Blog Not Found");
            }
            blog.Title = dto.Title;
            blog.Description = dto.Description;
            blog.AuthorId = dto.AuthorId;

            if (dto.ImageFile != null)
            {
                if (!dto.ImageFile.IsImage())
                {
                    commonResponse.StatusCode = 400;
                    commonResponse.Message = "Image is not valid";
                    return commonResponse;
                }

                if (dto.ImageFile.IsSizeOk(1))
                {
                    commonResponse.StatusCode = 400;
                    commonResponse.Message = "Image  size is not valid";
                    return commonResponse;
                }

                blog.Image = dto.ImageFile.SaveFile(_env.WebRootPath, "img/blog");
            }
            blog.TagBlogs.Clear();

            foreach (var item in dto.TagsIds)
            {
                blog.TagBlogs.Add(new TagBlog
                {
                    Blog = blog,
                    TagId = item,
                });
            }

            await _blogRepository.UpdateAsync(blog);
            await _blogRepository.SaveChangesAsync();
            return commonResponse;
        }
    }
}

