using System;
using Karma.Core.DTOS;

namespace Karma.Service.Services.Interfaces
{
	public interface IBrandService
	{

        public Task<IEnumerable<BrandGetDto>> GetAllAsync();

        public  Task CreateAsync(BrandPostDto dto);

        public Task RemoveAsync(int id);

        public  Task UpdateAsync(int id, BrandPostDto dto);
        public Task<BrandGetDto> GetAsync(int id);
       
    }
}


