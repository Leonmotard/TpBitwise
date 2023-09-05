using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TPBitwiseTraining.DAL.Interfaces;
using TPBitwiseTraining.DTO;
using TPBitwiseTraining.Models;

namespace TPBitwiseTraining.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly IGenericRepository<Brand> _repository;
        private readonly IMapper _mapper;

        public BrandController(IGenericRepository<Brand> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<BrandCreationDTO>> Create(BrandCreationDTO brandCreationDTO)
        {
            var brand = _mapper.Map<Brand>(brandCreationDTO);
            await _repository.Insert(brand);
            var brandDTO = _mapper.Map<BrandAnswerDTO>(brand);
            return Ok(brandDTO);
        }

    }
}
