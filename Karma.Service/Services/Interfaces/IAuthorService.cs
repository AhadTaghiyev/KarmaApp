using System;
using Karma.Core.DTOS;
using Karma.Service.Responses;

namespace Karma.Service.Services.Interfaces
{
	public interface IAuthorService
	{

        public Task<IEnumerable<AuthorGetDto>> GetAllAsync();

        public  Task<CommonResponse> CreateAsync(AuthorPostDto dto);

        public Task RemoveAsync(int id);

        public  Task<CommonResponse> UpdateAsync(int id, AuthorPostDto dto);
        public Task<AuthorGetDto> GetAsync(int id);
       
    }
}


