using System;
using Karma.Core.DTOS;
using Karma.Core.Entities;
using Karma.Core.Repositories;
using Karma.Data.Repositories;
using Karma.Service.Exceptions;
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
            if(! await CheckPosition(dto.PositionId))
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
            

            for (int i = 0; i < dto.Icons.Count(); i++)
            {
                SocialNetwork socialNetwork = new SocialNetwork
                {
                    Author = Author,
                    Icon = dto.Icons[i],
                    Url = dto.Urls[i]
                };
                Author.SocialNetworks.Add(socialNetwork);
            }
            Author.Storage = "wwwroot";
            string RootPath = Path.Combine(_env.WebRootPath, "img","author");
            string FileName = Guid.NewGuid().ToString() + dto.ImageFile.FileName;
            string FullPath = Path.Combine(RootPath, FileName);
            using (FileStream fileStream = new FileStream(FullPath, FileMode.Create))
            {
                dto.ImageFile.CopyTo(fileStream);
            }

            Author.Image = FileName;

            await _authorRepository.AddAsync(Author);
            await _authorRepository.SaveChangesAsync();
            return commonResponse;
        }

        public async Task<IEnumerable<AuthorGetDto>> GetAllAsync()
        {
            IEnumerable<AuthorGetDto> Authors = await _authorRepository.GetQuery(x => !x.IsDeleted)
               .AsNoTrackingWithIdentityResolution()
               .Include(x=>x.Position)
               .Select(x =>
               new AuthorGetDto {
                   FullName = x.FullName,
                   Id = x.Id,
                   position=new PositionGetDto { Name=x.Position.Name},
                   Image=x.Image
               })
               .ToListAsync();
            return Authors;
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
                Info=Author.Info,
                Icons=Author.SocialNetworks.Select(x=>x.Icon).ToList(),
                Urls = Author.SocialNetworks.Select(x => x.Url).ToList(),
                PositionId=Author.PositionId,
                position =new PositionGetDto { Name=Author.Position.Name}
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

            for (int i = 0; i < dto.Icons.Count(); i++)
            {
                SocialNetwork socialNetwork = new SocialNetwork
                {
                    Author = Author,
                    Icon = dto.Icons[i],
                    Url = dto.Urls[i]
                };
                Author.SocialNetworks.Add(socialNetwork);
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

