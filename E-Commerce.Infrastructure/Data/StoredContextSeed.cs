using E_Commerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.Data
{
    public class StoredContextSeed
    {

        public static async Task SeedAsync(ApplicationDbContext context)
        {
            if (!context.Categories.Any())
            {
                var categoriesData = File.ReadAllText("../E-Commerce.Infrastructure/Data/SeedData/categories.json");
                var categories = JsonSerializer.Deserialize<List<Category>>(categoriesData);
                foreach (var item in categories)
                {
                    context.Categories.Add(item);
                }
                await context.SaveChangesAsync();
            }
            if (!context.Brands.Any())
            {
                var brandsData = File.ReadAllText("../E-Commerce.Infrastructure/Data/SeedData/brands.json");
                var Brands = JsonSerializer.Deserialize<List<Brand>>(brandsData);
                foreach (var item in Brands)
                {
                    context.Brands.Add(item);
                }
                await context.SaveChangesAsync();
            }
            if (!context.Products.Any())
            {
                var prodsData = File.ReadAllText("../E-Commerce.Infrastructure/Data/SeedData/products.json");
                var products = JsonSerializer.Deserialize<List<Product>>(prodsData);
                foreach (var item in products)
                {
                    context.Products.Add(item);
                }
                await context.SaveChangesAsync();
            }

            if (!context.ProductDetails.Any())
            {
                var prodDetailsData = File.ReadAllText("../E-Commerce.Infrastructure/Data/SeedData/productDetails.json");
                var productDetails = JsonSerializer.Deserialize<List<ProductDetails>>(prodDetailsData);
                foreach (var item in productDetails)
                {
                    context.ProductDetails.Add(item);
                }
                await context.SaveChangesAsync();
            }

            if (!context.Reviews.Any())
            {
                var reviewsData = File.ReadAllText("../E-Commerce.Infrastructure/Data/SeedData/reviews.json");
                var reviews = JsonSerializer.Deserialize<List<Review>>(reviewsData);
                foreach (var item in reviews)
                {
                    context.Reviews.Add(item);
                }
                await context.SaveChangesAsync();
            }
            if (!context.DeliveryMethod.Any())
            {
                var DeliveryMethodData = File.ReadAllText("../E-Commerce.Infrastructure/Data/SeedData/delivery.json");
                var DeliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(DeliveryMethodData);
                foreach (var item in DeliveryMethods)
                {
                    context.DeliveryMethod.Add(item);
                }
                await context.SaveChangesAsync();
            }


        }
    }
}
