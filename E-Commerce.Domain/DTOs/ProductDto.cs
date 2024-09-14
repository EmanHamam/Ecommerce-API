using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace E_Commerce.Domain.DTOs
{
    public class ProductDto
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set;}
        public string ImageURL { get; set; }
        public string CategoryName { get; set;}
        public string BrandName { get; set; }
        public int CategoryId { get; set; }
        public int? Frequency {  get; set; }
        public double Rating { get; set; }


    }
    public class ProductDetailsDto
    {
        public int ProductDetailsID { get; set; }
        public int ProducID { get; set; }
        public string ImageURL { get; set; }
        public string smallImg1 { get; set; }
        public string smallImg2 { get; set; }
        public string smallImg3 { get; set; }
        public string ProductName { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string CategoryName { get; set; }
        public string BrandName { get; set; }
        public int CategoryId { get; set; }
        public double Rating { get; set; }
        public int numOfReviews {  get; set; }


    }
    public class ProductParams
    {
        private int MaxPageSize = 10;
        private int _pageSize   = 8;

        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = value > MaxPageSize ? MaxPageSize : value; }
        }


        private int _pageNumber = 1;
        public int PageNumber
        {
            get { return _pageNumber; }
            set { _pageNumber = value < 1 ? 1 : value; }
        }
    }
    public class ReturnProductDto
    {
        public int TotalItems { get; set; }
        public int PageSize {  get; set; }
        public int PageNumber { get; set; }
        public List<ProductDto> ProductDtos { get; set; }
    }

    public class CreateProductDto 
    {
        public string ProductName { get; set;}
        public string? Description { get; set;}
        
        public decimal Price { get; set; }
        public int StockQuantity { get; set;}
        public int CategoryId { get; set; }
        public int BrandId { get; set; }


        //[AllowedExtensionsAttribute(FileSettings._AllowedExtentions)
        //    , MaxFileSize(FileSettings._MaxFileSizeInBytes)]

        public IFormFile ImageURL { get; set; }
        
    }
    public class CreateProductDetailsDto
    {
        public int ProductID { get; set; }
        public IFormFile SmallImg1 { get; set; }
        public IFormFile SmallImg2 { get; set; }
        public IFormFile SmallImg3 { get; set; }

    }
    public class UpdateProductDto 
    {
      
        public string ProductName { get; set; }
        public string? Description { get; set; }
        public int BrandId { get; set; }

        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public int CategoryId { get; set; }
        //public string OldImage { get; set; }
        public IFormFile ImageURL { get; set; }
    }
    
}
