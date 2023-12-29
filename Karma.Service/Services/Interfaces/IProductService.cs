using System;
using Karma.Core.DTOS;
using Karma.Service.Responses;

namespace Karma.Service.Services.Interfaces
{
	public interface IProductService
	{

        public Task<IEnumerable<ProductGetDto>> GetAllAsync();

        public  Task<CommonResponse> CreateAsync(ProductPostDto dto);

        public Task RemoveAsync(int id);

        public  Task<CommonResponse> UpdateAsync(int id, ProductPostDto dto);
        public Task<ProductGetDto> GetAsync(int id);
       
    }
}


