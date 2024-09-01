using AutoMapper;
using E_Commerce.Domain.DTOs;
using E_Commerce.Domain.Entities;
using E_Commerce.Infrastructure.Data;
using E_Commerce.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.Repositories.Services
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _imgPath;
     
        public ProductRepository(ApplicationDbContext context, 
            IMapper mapper, IWebHostEnvironment webHostEnvironment) : base(context)
        {
            _context = context;
            _mapper = mapper;
            _webHostEnvironment=webHostEnvironment;
            _imgPath = $"{_webHostEnvironment.WebRootPath}/images/products";

        }

        //public async Task<ResponseDto> GetAllProducts()
        //{
        //    var Products= await _context.Products
        //        .Include(p => p.Category)
        //        .ToListAsync();
        //    if (!Products.Any())
        //    {
        //        return new ResponseDto
        //        {
        //            StatusCode = 400,
        //            IsSucceeded = false,
        //            DisplayMessage = "There are no products."
        //        };
        //    }

        //    var ProductsDto = _mapper.Map<List<ProductDto>>(Products);

        //    return new ResponseDto
        //    {
        //        StatusCode = 200,
        //        IsSucceeded = true,
        //        Result = ProductsDto,
                
        //    };
        //}
        public async Task<ResponseDto> GetAllProducts(ProductParams? productParams)
        {
            var result = new ReturnProductDto();

            if (productParams.PageNumber < 1)
            {
                productParams.PageNumber = 1;
            }
            var query = _context.Products
                .Include(p => p.Category)
                .AsNoTracking();

            result.TotalItems = await query.CountAsync();

            // Apply pagination
            var pagedQuery = await query
                .Skip((productParams.PageNumber - 1) * productParams.PageSize)
                .Take(productParams.PageSize)
                .ToListAsync();

            
            if (pagedQuery == null || !pagedQuery.Any())
            {
                return new ResponseDto
                {
                    StatusCode = 400,
                    IsSucceeded = false,
                    DisplayMessage = "There are no products"
                };
            }

            
            result.ProductDtos = _mapper.Map<List<ProductDto>>(pagedQuery);
            result.PageNumber = productParams.PageNumber;
            result.PageSize = productParams.PageSize;


            return new ResponseDto
            {
                StatusCode = 200,
                IsSucceeded = true,
                Result = result,
            };
        }
        public async Task<ResponseDto> GetProductById(int id)
        {
            var Product = await _context.Products
                .Where(p => p.ProductID == id)
               .Include(p => p.Category)
               .FirstOrDefaultAsync();
            if(Product == null)
            {
                return new ResponseDto
                {
                    StatusCode = 400,
                    IsSucceeded = false,
                    DisplayMessage = $"Product with ID {id} doesn't exist"
                };
            }
            var ProductDto = _mapper.Map<ProductDto>(Product);
            return new ResponseDto
            {
                StatusCode = 200,
                IsSucceeded = true,
               Result= ProductDto
            };

        }
        public async Task<ResponseDto> GetProductDetailsById(int prdId)
        {
            var ProductDetails = await _context.ProductDetails
                .Include(pd => pd.Product)
                .ThenInclude(p=>p!.Category)
                .Where(pd => pd.ProducID == prdId)
                .FirstOrDefaultAsync();

           
            if (ProductDetails == null)
            {
                return new ResponseDto
                {
                    StatusCode = 400,
                    IsSucceeded = false,
                    DisplayMessage = $"Product with ID {prdId} not found"
                };
            }
            var ProductDto = _mapper.Map<ProductDetailsDto>(ProductDetails);
            var reviews=  await _context.Reviews.Where(r => r.ProductID == prdId).ToListAsync();
            var rate = await _context.Reviews.Where(r => r.ProductID == prdId).AverageAsync(r => r.Rating);

            ProductDto.numOfReviews = reviews.Count;
            ProductDto.Rating = rate;
            
            return new ResponseDto
            {
                StatusCode = 200,
                IsSucceeded = true,
                Result = ProductDto
            };
        }

        //public async Task<ResponseDto> GetProductsByCategoryId(int categoryId)
        //{
        //    var Products=await _context.Products
        //        .Where(p => p.CategoryID == categoryId)
        //        .Include(c=>c.Category)
        //        .ToListAsync();
        //    if (Products == null||Products.Count==0)
        //    {
        //        return new ResponseDto
        //        {
        //            StatusCode = 400,
        //            IsSucceeded = false,
        //            DisplayMessage = "no Products were found for this Category"
        //        };
        //    }

        //    var ProductsDto = _mapper.Map<List<ProductDto>>(Products);

        //    return new ResponseDto
        //    {
        //        StatusCode = 200,
        //        IsSucceeded = true,
        //        Result = ProductsDto,

        //    };

        //}
        public async Task<ResponseDto> GetProductsByCategoryId(int categoryId, ProductParams? productParams)
        {
            var result = new ReturnProductDto();

            if (productParams.PageNumber < 1)
            {
                productParams.PageNumber = 1;
            }
            var query = _context.Products
                 .Where(p => p.CategoryID == categoryId)
                .Include(p => p.Category)
                .AsNoTracking();


            result.TotalItems = await query.CountAsync();

            // Apply pagination
            var pagedQuery = await query
                .Skip((productParams.PageNumber - 1) * productParams.PageSize)
                .Take(productParams.PageSize)
                .ToListAsync();


            if (pagedQuery == null || !pagedQuery.Any())
            {
                return new ResponseDto
                {
                    StatusCode = 400,
                    IsSucceeded = false,
                    DisplayMessage = "There are no products"
                };
            }


            result.ProductDtos = _mapper.Map<List<ProductDto>>(pagedQuery);
            result.PageNumber = productParams.PageNumber;
            result.PageSize=productParams.PageSize;

            return new ResponseDto
            {
                StatusCode = 200,
                IsSucceeded = true,
                Result = result,
            };



        }

        public async Task<ResponseDto> GetProductsByBrand(string brand)
        {
            var Products = await _context.Products
                .Where(p => p.Brand.BrandName == brand)
                .Include(c => c.Category)
                .ToListAsync();
            if (Products == null)
            {
                return new ResponseDto
                {
                    StatusCode = 400,
                    IsSucceeded = false,
                    DisplayMessage = "no Products were found for this Brand"
                };
            }

            var ProductsDto = _mapper.Map<List<ProductDto>>(Products);

            return new ResponseDto
            {
                StatusCode = 200,
                IsSucceeded = true,
                Result = ProductsDto,

            };


        }
        public async Task<IEnumerable<int>> GetAllIDs()
        {
            
            var ids = await _context.Products
                .Select(p => p.ProductID)
                .ToListAsync();

          
            return ids ?? new List<int>();
        }

        public async Task<ResponseDto> AddProduct(CreateProductDto dto)
        {
            try
            {
                var ImageName = await SaveImage(dto.ImageURL);

                var res = _mapper.Map<Product>(dto);
                res.Category = await _context.Categories.FirstOrDefaultAsync(c=>c.CategoryID==dto.CategoryId);
                res.ImageURL = ImageName;
                await _context.Products.AddAsync(res);

                await _context.SaveChangesAsync();
                return new ResponseDto
                {
                    StatusCode = 200,
                    IsSucceeded = true,
                    Result = res,
                };

            }
            catch (Exception ex)
            {
                return new ResponseDto
                {
                    StatusCode = 500,
                    IsSucceeded = false,
                    DisplayMessage = $"An error occurred: {ex.Message} and Unable to Add Product"
                };
            }


        }

        public async Task<ResponseDto> AddProductDetails(CreateProductDetailsDto dto)
        {
            //Identity for prddetailsid
            var productExists = await _context.Products.FirstOrDefaultAsync(p => p.ProductID == dto.ProductID);
            if (productExists==null)
            {
                return new ResponseDto
                {
                    StatusCode = 400,
                    IsSucceeded = false,
                    DisplayMessage = "Invalid ProductID"
                };
            }
            try{
                var imageName1 = dto.SmallImg1 != null ? await SaveImage(dto.SmallImg1) : productExists.ImageURL;
                var imageName2 = dto.SmallImg2 != null ? await SaveImage(dto.SmallImg2) : productExists.ImageURL;
                var imageName3 = dto.SmallImg3 != null ? await SaveImage(dto.SmallImg3) : productExists.ImageURL;


                var res = new ProductDetails
                {
                    ProducID = dto.ProductID,
                    SmallImg1 = imageName1,
                    SmallImg2 = imageName2,
                    SmallImg3 = imageName3,
                    Product = await _context.Products.FirstOrDefaultAsync(p => p.ProductID == dto.ProductID)

                };

                await _context.ProductDetails.AddAsync(res);
                await _context.SaveChangesAsync();
                return new ResponseDto
                {
                    StatusCode = 200,
                    IsSucceeded = true,
                    Result = res,
                };
            }

            catch (Exception ex)
            {
                return new ResponseDto
                {
                    StatusCode = 500,
                    IsSucceeded = false,
                    DisplayMessage = $"An error occurred: {ex.Message}"
                };
            }
        }
        public async Task<ResponseDto> UpdateProduct(int id, UpdateProductDto dto)
        {
            var Product = await _context.Products.FindAsync(id);
            if (Product == null)
            {
                return new ResponseDto
                {
                    StatusCode = 400,
                    IsSucceeded = false,
                    DisplayMessage = $"Product with ID {id} Not Found"
                };
            }
            var OldImage = Product.ImageURL;

           // var res = _mapper.Map<Product>(dto);
            if (dto.ImageURL is not null)
            {
                Product.ImageURL = await SaveImage(dto.ImageURL!);
            }
            
            var effectedRows= await _context.SaveChangesAsync();
            if (effectedRows > 0)
            {
                if(dto.ImageURL is not null && !string.IsNullOrEmpty(OldImage))
                {
                    var Image = Path.Combine(_imgPath, OldImage);
                    File.Delete(Image);
                }

                var res = _mapper.Map<Product>(dto);
                res.ImageURL = Product.ImageURL;
                res.ProductID= Product.ProductID;
                await _context.SaveChangesAsync();
                return new ResponseDto
                {
                    StatusCode = 200,
                    IsSucceeded = true,
                    Result = res,
                };
            }

            else
            {
                var Image = Path.Combine(_imgPath, Product.ImageURL);
                File.Delete(Image);

                return new ResponseDto
                {
                    StatusCode = 400,
                    IsSucceeded = false,
                    DisplayMessage = $"Can't Update Product"
                };
            }

        }
        public async Task<ResponseDto> UpdateProductDetails(int id, CreateProductDetailsDto dto)
        {

            var productDetails = await _context.ProductDetails.FirstOrDefaultAsync(pd => pd.ProducID == id);
            if (productDetails == null)
            {
                return new ResponseDto
                {
                    StatusCode = 404,
                    IsSucceeded = false,
                    DisplayMessage = "Product details not found."
                };
            }

            var image1 = productDetails.SmallImg1;
            var image2 = productDetails.SmallImg2;
            var image3 = productDetails.SmallImg3;
            if (dto.SmallImg1 != null)
            {
                productDetails.SmallImg1 = await SaveImage(dto.SmallImg1);
            }
            if (dto.SmallImg2 != null)
            {
                productDetails.SmallImg2 = await SaveImage(dto.SmallImg2);
            }
            if (dto.SmallImg3 != null)
            {
                productDetails.SmallImg3 = await SaveImage(dto.SmallImg3);
            }

            var effectedRows = await _context.SaveChangesAsync();
            if (effectedRows > 0)
            {
                if (dto.SmallImg1 is not null && !string.IsNullOrEmpty(image1))
                {
                    var Image = Path.Combine(_imgPath, image1);
                    File.Delete(Image);
                }
                if (dto.SmallImg2 is not null && !string.IsNullOrEmpty(image2))
                {
                    var Image = Path.Combine(_imgPath, image2);
                    File.Delete(Image);
                }
                if (dto.SmallImg3 is not null && !string.IsNullOrEmpty(image3))
                {
                    var Image = Path.Combine(_imgPath, image3);
                    File.Delete(Image);
                }
                //DeleteImageIfExists(image1);
                //DeleteImageIfExists(image2);
                //DeleteImageIfExists(image3);

                var res = _mapper.Map<ProductDetails>(dto);
                res.SmallImg1 = productDetails.SmallImg1;
                res.SmallImg2 = productDetails.SmallImg2;
                res.SmallImg3 = productDetails.SmallImg3;
                res.ProductDetailsID = productDetails.ProductDetailsID;
                res.ProducID = productDetails.ProducID;
                await _context.SaveChangesAsync();
                return new ResponseDto
                {
                    StatusCode = 200,
                    IsSucceeded = true,
                    Result = res,
                };
            }

            else
            {
                DeleteImageIfExists(productDetails.SmallImg1);
                DeleteImageIfExists(productDetails.SmallImg2);
                DeleteImageIfExists(productDetails.SmallImg3);

                return new ResponseDto
                {
                    StatusCode = 400,
                    IsSucceeded = false,
                    DisplayMessage = $"Can't Update Product"
                };
            }

        }
        public async Task<ResponseDto> DeleteProduct(int id)
        {
            var Product = await _context.Products.FindAsync(id);
            if (Product == null)
            {
                return new ResponseDto
                {
                    StatusCode = 400,
                    IsSucceeded = false,
                    DisplayMessage = $"Product with ID {id} Not Found"    
                };
            }
            var OldImage = Product.ImageURL;
            var productDetails = await _context.ProductDetails
        .FirstOrDefaultAsync(pd => pd.ProducID == id);

            if (productDetails != null)
            {
                if (!string.IsNullOrEmpty(productDetails.SmallImg1))
                {
                    DeleteImageIfExists(productDetails.SmallImg1);
                }
                if (!string.IsNullOrEmpty(productDetails.SmallImg2))
                {
                    DeleteImageIfExists(productDetails.SmallImg2);
                }
                if (!string.IsNullOrEmpty(productDetails.SmallImg3))
                {
                    DeleteImageIfExists(productDetails.SmallImg3);
                }
                _context.ProductDetails.Remove(productDetails);
            }

            _context.Products.Remove(Product);

            var effectedRows = await _context.SaveChangesAsync();
            if (effectedRows > 0)
            {
                DeleteImageIfExists(OldImage);
                return new ResponseDto
                {
                    StatusCode = 200,
                    IsSucceeded = true,
                    Result = Product,
                };
            }

            else
            {
               
                return new ResponseDto
                {
                    StatusCode = 400,
                    IsSucceeded = false,
                    DisplayMessage = $"Can't Delete Product"
                };
            }
        }


        private async Task<string> SaveImage(IFormFile ImageURL)
        {
            if (!Directory.Exists(_imgPath))
            {
                Directory.CreateDirectory(_imgPath);
            }
            var ImageName = $"{Guid.NewGuid()}{Path.GetExtension(ImageURL.FileName.ToLower())}";
            var path = Path.Combine(_imgPath, ImageName);
            using var stream = File.Create(path);
            await ImageURL.CopyToAsync(stream);
            return ImageName;
        }
        private void DeleteImageIfExists(string imagePath)
        {
            if (!string.IsNullOrEmpty(imagePath))
            {
                var fullPath = Path.Combine(_imgPath, imagePath);
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }
            }
        }

       
    }
}
