using System;
using Karma.Core.DTOS;
using Karma.Core.Entities;
using Karma.Core.Repositories;
using Karma.Data.Repositories;
using Karma.Service.Exceptions;
using Karma.Service.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Karma.Service.Services.Implementations
{
    public class TagService : ITagService
    {
        readonly ITagRepository _TagRepository;

        public TagService(ITagRepository TagRepository)
        {
            _TagRepository = TagRepository;
        }

        public async Task CreateAsync(TagPostDto dto)
        {
            Tag Tag = new Tag();
            Tag.Name = dto.Name;
            await _TagRepository.AddAsync(Tag);
            await _TagRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<TagGetDto>> GetAllAsync()
        {
            IEnumerable<TagGetDto> Tags = await _TagRepository.GetQuery(x => !x.IsDeleted)
               .AsNoTrackingWithIdentityResolution().Select(x => new TagGetDto { Name = x.Name, Id = x.Id, CreatedAt = x.CreatedAt })
               .ToListAsync();
            return Tags;
        }

        public async Task<TagGetDto> GetAsync(int id)
        {
            Tag? Tag = await _TagRepository.GetAsync(x => !x.IsDeleted && x.Id == id);

            if (Tag == null)
            {
                throw new ItemNotFoundExcpetion("Tag Not Found");
            }

            TagGetDto TagGetDto = new TagGetDto
            {
                CreatedAt = Tag.CreatedAt,
                Id = Tag.Id,
                Name = Tag.Name
            };
            return TagGetDto;
        }

        public async Task RemoveAsync(int id)
        {
            Tag? Tag = await _TagRepository.GetAsync(x => !x.IsDeleted && x.Id == id);

            if (Tag == null)
            {
                throw new ItemNotFoundExcpetion("Tag Not Found");
            }
            Tag.IsDeleted = true;
            await _TagRepository.UpdateAsync(Tag);
            await _TagRepository.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, TagPostDto dto)
        {
            Tag? Tag = await _TagRepository.GetAsync(x => !x.IsDeleted && x.Id == id);

            if (Tag == null)
            {
                throw new ItemNotFoundExcpetion("Tag Not Found");
            }

            Tag.Name = dto.Name;
            await _TagRepository.UpdateAsync(Tag);
            await _TagRepository.SaveChangesAsync();
        }
    }
}

