using System;
using Karma.Core.DTOS;
using Karma.Service.Responses;

namespace Karma.Service.Services.Interfaces
{
	public interface IAuthorService
	{

        public Task<PagginatedResponse<AuthorGetDto>> GetAllAsync(int page = 1);

        public  Task<CommonResponse> CreateAsync(AuthorPostDto dto);

        public Task RemoveAsync(int id);

        public  Task<CommonResponse> UpdateAsync(int id, AuthorPostDto dto);
        public Task<AuthorGetDto> GetAsync(int id);
       
    }
}


