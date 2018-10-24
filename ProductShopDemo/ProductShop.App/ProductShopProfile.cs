using AutoMapper;
using ProductShop.App.Dto.Import;
using ProductShop.Models;

namespace ProductShop.App
{
    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            CreateMap<UserDto, User>().ReverseMap();
            CreateMap<ProductDto, Product>().ReverseMap();
            CreateMap<CategoryDto, Category>().ReverseMap();
            CreateMap<Dto.Export.SoldProductDto, Product>().ReverseMap();
        }
    }
}
