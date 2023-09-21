using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
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
        protected ResponseApi _responseApi;

        public BrandController(IGenericRepository<Brand> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
            this._responseApi = new();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BrandAnswerDTO>>> GetAll()
        {
            var brands = await _repository.GetAll();
            var brandsDto = _mapper.Map<IEnumerable<BrandAnswerDTO>>(brands);
            return Ok(brandsDto);
        }

        [HttpPost]
        public async Task<ActionResult<BrandCreationDTO>> Create(BrandCreationDTO brandCreationDTO)
        {
            var brand = _mapper.Map<Brand>(brandCreationDTO);
            await _repository.Insert(brand);
            var brandDTO = _mapper.Map<BrandAnswerDTO>(brand);
            return CreatedAtAction(nameof(GetAll), new { id = brand.Id }, brandDTO);

        }



        [HttpDelete("{id}")]

        public async Task<ActionResult> Delete(int id)
        {
            var brandDb = await _repository.GetById(id);

            if (brandDb == null)
            {
                _responseApi.StatusCode = HttpStatusCode.NotFound;
                _responseApi.IsSuccess = false;
                _responseApi.ErrorMenssages.Add("The brand is not registrered in data base");
                return NotFound(_responseApi);
            }

            var result = await _repository.Delete(id);

            if (result)    
                return NoContent();

            _responseApi.StatusCode = HttpStatusCode.BadRequest;
            _responseApi.IsSuccess = false;
            _responseApi.ErrorMenssages.Add("An error has ocurred");
            return BadRequest(_responseApi);

        }


        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, BrandCreationDTO brandCreationDTO)
        {
            var brandDb = await _repository.GetById(id);
            if (brandDb == null)
            {
                _responseApi.StatusCode = HttpStatusCode.NotFound;
                _responseApi.IsSuccess = false;
                _responseApi.ErrorMenssages.Add("The brand is not registrered in data base");
                return NotFound(_responseApi);
            }

            _mapper.Map(brandCreationDTO, brandDb);
            
            var resultado = await _repository.Update(brandDb);
            if (resultado)
                return NoContent();


            _responseApi.StatusCode = HttpStatusCode.BadRequest;
            _responseApi.IsSuccess = false;
            _responseApi.ErrorMenssages.Add("An error has ocurred");
            return BadRequest(_responseApi);
        }
    }
}
