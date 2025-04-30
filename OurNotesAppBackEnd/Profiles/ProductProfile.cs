using AutoMapper;
using OurNotesAppBackEnd.Dtos.Comment;
using OurNotesAppBackEnd.Dtos.Product;
using OurNotesAppBackEnd.Models;

namespace OurNotesAppBackEnd.Profiles;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        //Source -> Target
        CreateMap<Product, ProductReadDto>();
        CreateMap<ProductCreateDto, Product>();
        CreateMap<ProductUpdateDto, Product>();
    }
}