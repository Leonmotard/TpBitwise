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

        }

    }
}
