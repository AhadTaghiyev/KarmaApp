using System;
using Karma.Core.DTOS;

namespace Karma.Service.Services.Interfaces
{
	public interface ITagService
	{

        public Task<IEnumerable<TagGetDto>> GetAllAsync();

        public  Task CreateAsync(TagPostDto dto);

        public Task RemoveAsync(int id);

        public  Task UpdateAsync(int id, TagPostDto dto);
        public Task<TagGetDto> GetAsync(int id);
       
    }
}


