using AutoMapper;
using E_Commerce.Domain.DTOs;
using E_Commerce.Domain.Entities;
using static E_Commerce.Domain.DTOs.ProductDto;

namespace E_Commerce.Mapping_Profiles
{
    public class MappingProfiles:Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductDto>()
                .ForMember(d => d.CategoryName, o => o.MapFrom(s => s.Category!.CategoryName))
                .ForMember(d => d.BrandName, o => o.MapFrom(s => s.Brand!.BrandName))
                .ReverseMap();

            CreateMap<ProductDetails, ProductDetailsDto>()
                .ForMember(d => d.CategoryName, o => o.MapFrom(s => s.Product!.Category!.CategoryName))
                .ForMember(d => d.BrandName, o => o.MapFrom(s => s.Product!.Brand!.BrandName))
                .ForMember(d => d.CategoryId  ,o => o.MapFrom(s => s.Product!.Category!.CategoryID))
                .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product!.ProductName))
                .ForMember(d => d.Description, o => o.MapFrom(s => s.Product!.Description))
                .ForMember(d => d.StockQuantity, o => o.MapFrom(s => s.Product!.StockQuantity))
                .ForMember(d => d.Price, o => o.MapFrom(s => s.Product!.Price))
                .ForMember(d => d.ImageURL, o => o.MapFrom(s => s.Product!.ImageURL))
                 
                .ReverseMap();

            CreateMap<CreateProductDto, Product>().ReverseMap();

            CreateMap<CreateProductDetailsDto, ProductDetails>().ReverseMap();


            CreateMap<UpdateProductDto,Product>()
                //.ForMember(d => d.OldImage, o => o.MapFrom(s => s.ImageURL))
                .ReverseMap();

            //Reviews
            CreateMap<Review, ReviewDto>()
                .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product!.ProductName))
                .ReverseMap();

            CreateMap<Review, CreateReviewDto>()
                .ReverseMap();
            //Cart 

            CreateMap<ShoppingCart,CartDto>().ReverseMap();
            CreateMap<ShoppingCart, UpdateCartDto>().ReverseMap();

            CreateMap<ShoppingCart, ReturnCartDto>().ReverseMap();


            //CartItem
            CreateMap<CartItem, CartItemDto>().ReverseMap();

            CreateMap<CartItem, ReturnCartItemDto>()
                .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product!.ProductName))
                .ForMember(d => d.ProductImage, o => o.MapFrom(s => s.Product!.ImageURL))
                .ForMember(d => d.Price, o => o.MapFrom(s => s.Product!.Price))
                .ForMember(d => d.StockQuantity, o => o.MapFrom(s => s.Product!.StockQuantity))

                .ReverseMap();


            //wishlist
            CreateMap<Wishlist, WishlistDto>().ReverseMap();
            CreateMap<Wishlist, ReturnWishlistDto>().ReverseMap();


            //wishlistItem
            CreateMap<WishlistItem, WishlistItemDto>().ReverseMap();
            CreateMap<WishlistItem, ReturnWishlistItemDto>()
                 .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product!.ProductName))
                .ForMember(d => d.ProductImage, o => o.MapFrom(s => s.Product!.ImageURL))
                .ForMember(d => d.Price, o => o.MapFrom(s => s.Product!.Price))
                .ReverseMap();


            //Order
            CreateMap<Order, CreateOrderDto>()
                

                .ReverseMap();
            CreateMap<Order, OrderDto>()
                .ForMember(d => d.Total, o => o.MapFrom(s => s.Total))
                .ForMember(d => d.DeliveryMethodName, o => o.MapFrom(s => s.DeliveryMethod!.ShortName))
                .ForMember(d => d.OrderStatus, o => o.MapFrom(s => s.OrderStatus))
                .ForMember(d => d.DeliveringDate, o => o.MapFrom(s => s.DeliveryMethod!.DeliveryTime))
                .ForMember(d => d.DeliveryPrice, o => o.MapFrom(s => s.DeliveryMethod!.Price))


                .ReverseMap();
            //OrderItem
            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product!.ProductName))
                .ForMember(d => d.ImageUrl, o => o.MapFrom(s => s.Product!.ImageURL))
                .ReverseMap();
            CreateMap<OrderItem, OrderItemCreateDto>()
               
                .ReverseMap();
            //Adress
            CreateMap<Address, AddressDto>().ReverseMap();
            //ordershipingaddress
            CreateMap<OrderShippingAddress, OrderShippingAddressDto>().ReverseMap();





        }
    }
}
