using AutoMapper;
using Pawfect_Backend.Dto;
using Pawfect_Backend.Models;

namespace Pawfect_Backend.Helpers
{
    public class MappingProfile:Profile
    {
        public MappingProfile() { 
            CreateMap<User,LoginDto>().ReverseMap();
            CreateMap<User,SignUpDto>().ReverseMap();
            CreateMap<GetProductsDto, Product>().ReverseMap()
            .ForMember(e => e.CategoryName, e => e.MapFrom(e => e.category.CategoryName));
            CreateMap<User, GetAllUsersDto>().ReverseMap();
            CreateMap<Category, AddCategoryDto>().ReverseMap();
            CreateMap<Product, AddProductDto>().ReverseMap();
            CreateMap<WishList, getWishListDto>().ReverseMap();
            CreateMap<Cart,CartViewDto>().ReverseMap();
            CreateMap<CartItem,CartItemViewDto> ().ReverseMap();
            CreateMap<Address,AddressCreateDto>().ReverseMap();
            CreateMap<Address, GetAddressDto>().ReverseMap();
            CreateMap<OrderItem, OrderViewDto>()
                   .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalPrice))
                   .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.ProductName))
                   .ForMember(dest => dest.Url, opt => opt.MapFrom(src => src.Product.Url))
                   .ForMember(dest => dest.price, opt => opt.MapFrom(src => src.Product.Price))
                   .ReverseMap();
            ;
        
        }
    }
}
