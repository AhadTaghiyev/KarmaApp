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
    public class CategoryService : ICategoryService
    {
        readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task CreateAsync(CategoryPostDto dto)
        {
            Category category = new Category();
            category.Name = dto.Name;
            await _categoryRepository.AddAsync(category);
            await _categoryRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<CategoryGetDto>> GetAllAsync()
        {
            IEnumerable<CategoryGetDto> Categorys = await _categoryRepository.GetQuery(x => !x.IsDeleted)
               .AsNoTrackingWithIdentityResolution().Select(x => new CategoryGetDto { Name = x.Name, Id = x.Id, CreatedAt = x.CreatedAt })
               .ToListAsync();
            return Categorys;
        }

        public async Task<CategoryGetDto> GetAsync(int id)
        {
            Category? Category = await _categoryRepository.GetAsync(x => !x.IsDeleted && x.Id == id);

            if (Category == null)
            {
                throw new ItemNotFoundExcpetion("Category Not Found");
            }

            CategoryGetDto categoryGetDto = new CategoryGetDto
            {
                CreatedAt = Category.CreatedAt,
                Id = Category.Id,
                Name = Category.Name
            };
            return categoryGetDto;
        }

        public async Task RemoveAsync(int id)
        {
            Category? Category = await _categoryRepository.GetAsync(x => !x.IsDeleted && x.Id == id);

            if (Category == null)
            {
                throw new ItemNotFoundExcpetion("Category Not Found");
            }
            Category.IsDeleted = true;
            await _categoryRepository.UpdateAsync(Category);
            await _categoryRepository.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, CategoryPostDto dto)
        {
            Category? Category = await _categoryRepository.GetAsync(x => !x.IsDeleted && x.Id == id);

            if (Category == null)
            {
                throw new ItemNotFoundExcpetion("Category Not Found");
            }

            Category.Name = dto.Name;
            await _categoryRepository.UpdateAsync(Category);
            await _categoryRepository.SaveChangesAsync();
        }
    }
}

