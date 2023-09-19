using AutoMapper;
using TPBitwiseTraining.DTO;
using TPBitwiseTraining.Models;

namespace TPBitwiseTraining.Utils
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<BrandCreationDTO, Brand>().ReverseMap();
            CreateMap<BrandAnswerDTO, Brand>().ReverseMap();
            
            CreateMap<CategoryCreationDTO, Category>().ReverseMap();
            CreateMap<CategoryAnswerDTO, Category>().ReverseMap();

            CreateMap<ProductCreationDTO, Product>()
                .ForMember(d => d.Id, o=> o.Ignore())
                .ForMember(d => d.Category, o=>o.Ignore())
                .ForMember(d=> d.Brand, o => o.Ignore());

            CreateMap<Product, ProductListDTO>().
                ForMember(d => d.BrandName, o => o.MapFrom(src => src.Brand.Name));

            CreateMap<Product, ProductAnswerDTO>()
                .ForMember(d => d.BrandName, o => o.MapFrom(src => src.Brand.Name))
                .ForMember(d => d.CategoryName, o => o.MapFrom(src => src.Category.Name));
                
        }

    }
}
