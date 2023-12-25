using System;
using Karma.Core.DTOS;

namespace Karma.Service.Services.Interfaces
{
	public interface IPositionService
	{

        public Task<IEnumerable<PositionGetDto>> GetAllAsync();

        public  Task CreateAsync(PositionPostDto dto);

        public Task RemoveAsync(int id);

        public  Task UpdateAsync(int id, PositionPostDto dto);
        public Task<PositionGetDto> GetAsync(int id);
       
    }
}


