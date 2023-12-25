using System;
using Karma.Core.DTOS;

namespace Karma.Service.Services.Interfaces
{
	public interface ICategoryService
	{

        public Task<IEnumerable<CategoryGetDto>> GetAllAsync();

        public  Task CreateAsync(CategoryPostDto dto);

        public Task RemoveAsync(int id);

        public  Task UpdateAsync(int id, CategoryPostDto dto);
        public Task<CategoryGetDto> GetAsync(int id);
       
    }
}


