using System;
using Karma.Core.DTOS;

namespace Karma.Service.Services.Interfaces
{
	public interface IColorService
	{

        public Task<IEnumerable<ColorGetDto>> GetAllAsync();

        public  Task CreateAsync(ColorPostDto dto);

        public Task RemoveAsync(int id);

        public  Task UpdateAsync(int id, ColorPostDto dto);
        public Task<ColorGetDto> GetAsync(int id);
       
    }
}


