using System;
using Karma.Core.DTOS;
using Karma.Service.Responses;

namespace Karma.Service.Services.Interfaces
{
	public interface IContactService
	{

        public Task<IEnumerable<ContactGetDto>> GetAllAsync(int page = 1);

        public  Task<CommonResponse> CreateAsync(ContactPostDto dto);

        public Task RemoveAsync(int id);
    }
}


