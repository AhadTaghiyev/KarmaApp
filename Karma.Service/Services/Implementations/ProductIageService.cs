using System;
using Karma.Core.Repositories;
using Karma.Service.Exceptions;
using Karma.Service.Services.Interfaces;

namespace Karma.Service.Services.Implementations
{
	public class ProductImageService: IProductImageService
    {
		readonly IProductImageRepository productImageRepository;

        public ProductImageService(IProductImageRepository productImageRepository)
        {
            this.productImageRepository = productImageRepository;
        }

        public async Task RemoveAsync(int id)
        {
            var image = await productImageRepository.GetAsync(x=>x.Id==id&&x.IsDeleted==false);

            if(image == null){
                throw new ItemNotFoundExcpetion("The image not found");
            }

            image.IsDeleted = true;
            await productImageRepository.UpdateAsync(image);
            await productImageRepository.SaveChangesAsync();
        }
    }
}

