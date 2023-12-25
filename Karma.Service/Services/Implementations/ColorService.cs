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
    public class ColorService : IColorService
    {
        readonly IColorRepository _colorRepository;

        public ColorService(IColorRepository ColorRepository)
        {
            _colorRepository = ColorRepository;
        }

        public async Task CreateAsync(ColorPostDto dto)
        {
            Color Color = new Color();
            Color.Name = dto.Name;
            await _colorRepository.AddAsync(Color);
            await _colorRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<ColorGetDto>> GetAllAsync()
        {
            IEnumerable<ColorGetDto> Colors = await _colorRepository.GetQuery(x => !x.IsDeleted)
               .AsNoTrackingWithIdentityResolution().Select(x => new ColorGetDto { Name = x.Name, Id = x.Id, CreatedAt = x.CreatedAt })
               .ToListAsync();
            return Colors;
        }

        public async Task<ColorGetDto> GetAsync(int id)
        {
            Color? Color = await _colorRepository.GetAsync(x => !x.IsDeleted && x.Id == id);

            if (Color == null)
            {
                throw new ItemNotFoundExcpetion("Color Not Found");
            }

            ColorGetDto ColorGetDto = new ColorGetDto
            {
                CreatedAt = Color.CreatedAt,
                Id = Color.Id,
                Name = Color.Name
            };
            return ColorGetDto;
        }

        public async Task RemoveAsync(int id)
        {
            Color? Color = await _colorRepository.GetAsync(x => !x.IsDeleted && x.Id == id);

            if (Color == null)
            {
                throw new ItemNotFoundExcpetion("Color Not Found");
            }
            Color.IsDeleted = true;
            await _colorRepository.UpdateAsync(Color);
            await _colorRepository.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, ColorPostDto dto)
        {
            Color? Color = await _colorRepository.GetAsync(x => !x.IsDeleted && x.Id == id);

            if (Color == null)
            {
                throw new ItemNotFoundExcpetion("Color Not Found");
            }

            Color.Name = dto.Name;
            await _colorRepository.UpdateAsync(Color);
            await _colorRepository.SaveChangesAsync();
        }
    }
}

