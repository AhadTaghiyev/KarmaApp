using System;
using Karma.Core.DTOS;
using Karma.Core.Entities;
using Karma.Core.Repositories;
using Karma.Data.Repositories;
using Karma.Service.Exceptions;
using Karma.Service.Extentions;
using Karma.Service.Helpers;
using Karma.Service.Responses;
using Karma.Service.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace Karma.Service.Services.Implementations
{
    public class AuthorService : IAuthorService
    {
        readonly IAuthorRepository _authorRepository;
        readonly IPositionRepository _positionRepository;
        readonly IWebHostEnvironment _env;

        public AuthorService(IAuthorRepository AuthorRepository, IPositionRepository positionRepository, IWebHostEnvironment env)
        {
            _authorRepository = AuthorRepository;
            _positionRepository = positionRepository;
            _env = env;
        }

        public async Task<CommonResponse> CreateAsync(AuthorPostDto dto)
        {
            CommonResponse commonResponse = new CommonResponse
            {
                StatusCode = 200
            };

            if (dto.ImageFile == null)
            {
                commonResponse.StatusCode = 400;
                commonResponse.Message = "The field image is required";
                return commonResponse;
            }

            if (dto.Icons == null || dto.Urls == null || dto.Icons.Count() != dto.Urls.Count())
            {
                commonResponse.StatusCode = 400;
                commonResponse.Message = "The socihal network is not valid";
                return commonResponse;
            }

            if(dto.Icons.Any(x=>string.IsNullOrWhiteSpace(x))|| dto.Urls.Any(x => string.IsNullOrWhiteSpace(x)))
            {
                commonResponse.StatusCode = 400;
                commonResponse.Message = "The socihal network is not valid";
                return commonResponse;
            }

            if (! await CheckPosition(dto.PositionId))
            {
                commonResponse.StatusCode = 404;
                commonResponse.Message = "The Position is not valid";
                return commonResponse;
            }

            Author Author = new Author();
            Author.FullName = dto.FullName;
            Author.PositionId = dto.PositionId;
            Author.Info = dto.Info;
            Author.SocialNetworks = new List<SocialNetwork>();
            CreateSochial(dto,Author);

            Author.Storage = "wwwroot";
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

            Author.Image = dto.ImageFile.SaveFile(_env.WebRootPath, "img/author");
            await _authorRepository.AddAsync(Author);
            await _authorRepository.SaveChangesAsync();
            return commonResponse;
        }

        private void CreateSochial(AuthorPostDto dto,Author author)
        {
            for (int i = 0; i < dto.Icons.Count(); i++)
            {
                SocialNetwork socialNetwork = new SocialNetwork
                {
                    Author = author,
                    Icon = dto.Icons[i],
                    Url = dto.Urls[i]
                };
                author.SocialNetworks.Add(socialNetwork);
            }
        }

        public async Task<PagginatedResponse< AuthorGetDto>> GetAllAsync(int page=1)
        {
            PagginatedResponse<AuthorGetDto> pagginatedResponse = new PagginatedResponse<AuthorGetDto>();
            pagginatedResponse.CurrentPage = page;
            var query =  _authorRepository.GetQuery(x => !x.IsDeleted)
               .AsNoTrackingWithIdentityResolution()
               .Include(x => x.Position);
            pagginatedResponse.TotalPages = (int)Math.Ceiling((double)query.Count()/3);
     
            pagginatedResponse.Items=await query.Skip((page - 1) * 3)
               .Take(3)
               .Select(x =>
               new AuthorGetDto
               {
                   FullName = x.FullName,
                   Id = x.Id,
                   position = new PositionGetDto { Name = x.Position.Name },
                   Image = x.Image
               })
               .ToListAsync();

            return pagginatedResponse;
        }

        public async Task<AuthorGetDto> GetAsync(int id)
        {
            Author? Author = await _authorRepository.GetAsync(x => !x.IsDeleted && x.Id == id, "Position", "SocialNetworks");
                

            if (Author == null)
            {
                throw new ItemNotFoundExcpetion("Author Not Found");
            }

            AuthorGetDto AuthorGetDto = new AuthorGetDto
            {
                Id = Author.Id,
                FullName = Author.FullName,
                Info = Author.Info,
                Icons = Author.SocialNetworks.Select(x => x.Icon).ToList(),
                Urls = Author.SocialNetworks.Select(x => x.Url).ToList(),
                PositionId = Author.PositionId,
                position = new PositionGetDto { Name = Author.Position.Name },
                Image = Author.Image
            };
            return AuthorGetDto;
        }

        public async Task RemoveAsync(int id)
        {
            Author? Author = await _authorRepository.GetAsync(x => !x.IsDeleted && x.Id == id);

            if (Author == null)
            {
                throw new ItemNotFoundExcpetion("Author Not Found");
            }
            Author.IsDeleted = true;
            await _authorRepository.UpdateAsync(Author);
            await _authorRepository.SaveChangesAsync();
        }

        
        public async Task<CommonResponse> UpdateAsync(int id, AuthorPostDto dto)
        {
            CommonResponse commonResponse = new CommonResponse
            {
                StatusCode = 200
            };
            if (dto.Icons == null || dto.Urls == null || dto.Icons.Count() != dto.Urls.Count())
            {
                commonResponse.StatusCode = 400;
                commonResponse.Message = "The socihal network is not valid";
                return commonResponse;
            }

            if (dto.Icons.Any(x => string.IsNullOrWhiteSpace(x)) || dto.Urls.Any(x => string.IsNullOrWhiteSpace(x)))
            {
                commonResponse.StatusCode = 400;
                commonResponse.Message = "The socihal network is not valid";
                return commonResponse;
            }


            if (!await CheckPosition(dto.PositionId))
            {
                commonResponse.StatusCode = 404;
                commonResponse.Message = "The Position is not valid";
                return commonResponse;
            }

            Author? Author = await _authorRepository.GetAsync(x => !x.IsDeleted && x.Id == id, "SocialNetworks");

            if (Author == null)
            {
                throw new ItemNotFoundExcpetion("Author Not Found");
            }

            Author.FullName = dto.FullName;
            Author.Info = dto.Info;
            Author.PositionId = dto.PositionId;
            Author.SocialNetworks.Clear();
            CreateSochial(dto, Author);

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
                Helper.RmoveFile(_env.WebRootPath,"img/author",Author.Image);
                Author.Image = dto.ImageFile.SaveFile(_env.WebRootPath, "img/author");
            }

           await _authorRepository.UpdateAsync(Author);
            await _authorRepository.SaveChangesAsync();
            return commonResponse;
        }


        private async Task<bool> CheckPosition(int id)
        {
            return await _positionRepository.GetQuery(x => !x.IsDeleted && x.Id == id).CountAsync() > 0;
        }
    }
}

