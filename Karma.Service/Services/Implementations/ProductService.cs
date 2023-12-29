using System;
using Karma.Core.DTOS;
using Karma.Core.Entities;
using Karma.Core.Repositories;
using Karma.Data.Repositories;
using Karma.Service.Exceptions;
using Karma.Service.Extentions;
using Karma.Service.Responses;
using Karma.Service.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace Karma.Service.Services.Implementations
{
    public class ProductService : IProductService
    {
        readonly IWebHostEnvironment _env;
        readonly IProductRepository _productRepository;

        public ProductService(IWebHostEnvironment env, IProductRepository productRepository)
        {
            _env = env;
            _productRepository = productRepository;
        }

        public async Task<CommonResponse> CreateAsync(ProductPostDto dto)
        {
            CommonResponse commonResponse = new CommonResponse();
            commonResponse.StatusCode = 200;


            if (dto.MainImageFile == null)
            {
                commonResponse.StatusCode = 400;
                commonResponse.Message = "The field image is required";
                return commonResponse;
            }
            if (dto.ProductImageFile == null||dto.ProductImageFile.Count()==0)
            {
                commonResponse.StatusCode = 400;
                commonResponse.Message = "The field image is required";
                return commonResponse;
            }

            if (dto.ColorIds == null || dto.Counts == null || dto.ColorIds.Count() != dto.Counts.Count())
            {
                commonResponse.StatusCode = 400;
                commonResponse.Message = "The Color  is not valid";
                return commonResponse;
            }


            if (dto.SpecificationKeys == null || dto.SpecificationValues == null || dto.SpecificationKeys.Count() != dto.SpecificationValues.Count())
            {
                commonResponse.StatusCode = 400;
                commonResponse.Message = "The socihal network is not valid";
                return commonResponse;
            }

            if (dto.SpecificationKeys.Any(x => string.IsNullOrWhiteSpace(x)) || dto.SpecificationValues.Any(x => string.IsNullOrWhiteSpace(x)))
            {
                commonResponse.StatusCode = 400;
                commonResponse.Message = "The socihal network is not valid";
                return commonResponse;
            }



            Product product = new Product
            {
                BrandId = dto.BrandId,
                CategoryId = dto.CategoryId,
                Description = dto.Description,
                Name = dto.Name,
                Info = dto.Info,
                Price = dto.Price,
                DiscountPrice = dto.DiscountPrice,
                ColorProducts = new List<ColorProduct>(),
                Specifications = new List<Specification>(),
                ProductImages = new List<ProductImage>(),
            };

            for (int i = 0; i < dto.ColorIds.Count(); i++)
            {
                product.ColorProducts.Add(new ColorProduct
                {
                    Product = product,
                    StockCount = dto.Counts[i],
                    ColorId = dto.ColorIds[i],
                });
            }
            for (int i = 0; i < dto.SpecificationKeys.Count(); i++)
            {
                product.Specifications.Add(new Specification
                {
                    Product = product,
                    Key = dto.SpecificationKeys[i],
                    Value = dto.SpecificationValues[i],
                });
            }

            if (!dto.MainImageFile.IsImage())
            {
                commonResponse.StatusCode = 400;
                commonResponse.Message = "Image is not valid";
                return commonResponse;
            }

            if (dto.MainImageFile.IsSizeOk(1))
            {
                commonResponse.StatusCode = 400;
                commonResponse.Message = "Image  size is not valid";
                return commonResponse;
            }

            string mainImage = dto.MainImageFile.SaveFile(_env.WebRootPath, "img/product");

            product.ProductImages.Add(new ProductImage
            {
                Image = mainImage,
                IsMain = true,
                Product = product,
            });

            foreach (var item in dto.ProductImageFile)
            {
                if (!item.IsImage())
                {
                    commonResponse.StatusCode = 400;
                    commonResponse.Message = "Image is not valid";
                    return commonResponse;
                }

                if (item.IsSizeOk(1))
                {
                    commonResponse.StatusCode = 400;
                    commonResponse.Message = "Image  size is not valid";
                    return commonResponse;
                }

                string image = item.SaveFile(_env.WebRootPath, "img/product");

                product.ProductImages.Add(new ProductImage
                {
                    Image = image,
                    IsMain = false,
                    Product = product,
                });

            }

            await _productRepository.AddAsync(product);
            await _productRepository.SaveChangesAsync();

            return commonResponse; 
        }


        public async Task<IEnumerable<ProductGetDto>> GetAllAsync()
        {
            var query = _productRepository.GetQuery(x => x.IsDeleted == false)
                .Include(x => x.Category)
                .Include(x => x.ProductImages);
            IEnumerable<ProductGetDto> productGetDtos = query.Select(x =>
            new ProductGetDto
            {
                Id=x.Id,
                Name=x.Name,
                Category=new CategoryGetDto { Name=x.Category.Name},
                Price=x.Price,
                DiscountPrice=x.DiscountPrice,
                ProductImage=x.ProductImages
            }
            );
            return productGetDtos;
        }

        public async Task<ProductGetDto> GetAsync(int id)
        {
            var query = _productRepository.GetQuery(x => x.IsDeleted == false&&x.Id==id)
                .Include(x => x.ProductImages)
                .Include(x => x.Category)
                .Include(x => x.Brand)
                .Include(x => x.Specifications)
                .Include(x => x.ColorProducts)
                .ThenInclude(x => x.Color);
                

          
            ProductGetDto? product = await query.Select(x => new ProductGetDto
            {
                Specifications=x.Specifications,
                Brand=new BrandGetDto { Name=x.Brand.Name},
                BrandId=x.BrandId,
                CategoryId=x.CategoryId,
                Category=new CategoryGetDto { Name=x.Category.Name},
                ColorProducts=x.ColorProducts,
                Description=x.Description,
                DiscountPrice=x.DiscountPrice,
                Id=x.Id,
                Info=x.Info,
                Name=x.Name,
                Price=x.Price,
                ProductImage=x.ProductImages
            }).FirstOrDefaultAsync();
            if (product == null)
            {
                throw new ItemNotFoundExcpetion("Product Not Found");
            }

            return product;
        }

        public async Task RemoveAsync(int id)
        {
            Product? Product = await _productRepository.GetAsync(x => !x.IsDeleted && x.Id == id);

            if (Product == null)
            {
                throw new ItemNotFoundExcpetion("Product Not Found");
            }
            Product.IsDeleted = true;
            await _productRepository.UpdateAsync(Product);
            await _productRepository.SaveChangesAsync();
        }

        public async Task<CommonResponse> UpdateAsync(int id, ProductPostDto dto)
        {
            CommonResponse commonResponse = new CommonResponse();
            commonResponse.StatusCode = 200;


            if (dto.ColorIds == null || dto.Counts == null || dto.ColorIds.Count() != dto.Counts.Count())
            {
                commonResponse.StatusCode = 400;
                commonResponse.Message = "The Color  is not valid";
                return commonResponse;
            }


            if (dto.SpecificationKeys == null || dto.SpecificationValues == null || dto.SpecificationKeys.Count() != dto.SpecificationValues.Count())
            {
                commonResponse.StatusCode = 400;
                commonResponse.Message = "The socihal network is not valid";
                return commonResponse;
            }

            if (dto.SpecificationKeys.Any(x => string.IsNullOrWhiteSpace(x)) || dto.SpecificationValues.Any(x => string.IsNullOrWhiteSpace(x)))
            {
                commonResponse.StatusCode = 400;
                commonResponse.Message = "The socihal network is not valid";
                return commonResponse;
            }

            var product = await _productRepository.GetQuery(x => x.IsDeleted == false&&x.Id==id)
                  .Include(x => x.ProductImages)
                  .Include(x => x.Category)
                  .Include(x => x.Brand)
                  .Include(x => x.Specifications)
                  .Include(x => x.ColorProducts)
                  .ThenInclude(x => x.Color)
                  .FirstOrDefaultAsync();

            if (product == null)
            {
                throw new ItemNotFoundExcpetion("Product Not Found");
            }

            product.Name = dto.Name;
            product.Info = dto.Info;
            product.Description = dto.Description;
            product.BrandId = dto.BrandId;
            product.CategoryId = dto.CategoryId;
            product.DiscountPrice = dto.DiscountPrice;
            product.Price = dto.Price;
            product.Specifications.Clear();
            for (int i = 0; i < dto.SpecificationKeys.Count(); i++)
            {
                product.Specifications.Add(new Specification
                {
                    Product = product,
                    Key = dto.SpecificationKeys[i],
                    Value = dto.SpecificationValues[i],
                });
            }

            product.ColorProducts.Clear();

            for (int i = 0; i < dto.ColorIds.Count(); i++)
            {
                product.ColorProducts.Add(new ColorProduct
                {
                    Product = product,
                    StockCount = dto.Counts[i],
                    ColorId = dto.ColorIds[i],
                });
            }

            if (dto.ProductImageFile != null)
            {
                foreach (var item in dto.ProductImageFile)
                {
                    if (!item.IsImage())
                    {
                        commonResponse.StatusCode = 400;
                        commonResponse.Message = "Image is not valid";
                        return commonResponse;
                    }

                    if (item.IsSizeOk(1))
                    {
                        commonResponse.StatusCode = 400;
                        commonResponse.Message = "Image  size is not valid";
                        return commonResponse;
                    }

                    string image = item.SaveFile(_env.WebRootPath, "img/product");

                    product.ProductImages.Add(new ProductImage
                    {
                        Image = image,
                        IsMain = false,
                        Product = product,
                    });


                

                }
            }

            if (dto.MainImageFile != null)
            {
                if (!dto.MainImageFile.IsImage())
                {
                    commonResponse.StatusCode = 400;
                    commonResponse.Message = "Image is not valid";
                    return commonResponse;
                }

                if (dto.MainImageFile.IsSizeOk(1))
                {
                    commonResponse.StatusCode = 400;
                    commonResponse.Message = "Image  size is not valid";
                    return commonResponse;
                }

                string mainImage = dto.MainImageFile.SaveFile(_env.WebRootPath, "img/product");

                product.ProductImages.Remove(product.ProductImages.FirstOrDefault(x => x.IsMain));

                product.ProductImages.Add(new ProductImage
                {
                    Image = mainImage,
                    IsMain = true,
                    Product = product,
                });


            }

            await _productRepository.UpdateAsync(product);
           int count= await _productRepository.SaveChangesAsync();

            return commonResponse;
        }



       
    }
}

