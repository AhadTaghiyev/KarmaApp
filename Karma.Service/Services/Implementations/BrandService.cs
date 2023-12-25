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
    public class BrandService : IBrandService
    {
        readonly IBrandRepository _brandRepository;

        public BrandService(IBrandRepository BrandRepository)
        {
            _brandRepository = BrandRepository;
        }

        public async Task CreateAsync(BrandPostDto dto)
        {
            Brand Brand = new Brand();
            Brand.Name = dto.Name;
            await _brandRepository.AddAsync(Brand);
            await _brandRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<BrandGetDto>> GetAllAsync()
        {
            IEnumerable<BrandGetDto> Brands = await _brandRepository.GetQuery(x => !x.IsDeleted)
               .AsNoTrackingWithIdentityResolution().Select(x => new BrandGetDto { Name = x.Name, Id = x.Id, CreatedAt = x.CreatedAt })
               .ToListAsync();
            return Brands;
        }

        public async Task<BrandGetDto> GetAsync(int id)
        {
            Brand? Brand = await _brandRepository.GetAsync(x => !x.IsDeleted && x.Id == id);

            if (Brand == null)
            {
                throw new ItemNotFoundExcpetion("Brand Not Found");
            }

            BrandGetDto BrandGetDto = new BrandGetDto
            {
                CreatedAt = Brand.CreatedAt,
                Id = Brand.Id,
                Name = Brand.Name
            };
            return BrandGetDto;
        }

        public async Task RemoveAsync(int id)
        {
            Brand? Brand = await _brandRepository.GetAsync(x => !x.IsDeleted && x.Id == id);

            if (Brand == null)
            {
                throw new ItemNotFoundExcpetion("Brand Not Found");
            }
            Brand.IsDeleted = true;
            await _brandRepository.UpdateAsync(Brand);
            await _brandRepository.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, BrandPostDto dto)
        {
            Brand? Brand = await _brandRepository.GetAsync(x => !x.IsDeleted && x.Id == id);

            if (Brand == null)
            {
                throw new ItemNotFoundExcpetion("Brand Not Found");
            }

            Brand.Name = dto.Name;
            await _brandRepository.UpdateAsync(Brand);
            await _brandRepository.SaveChangesAsync();
        }
    }
}

