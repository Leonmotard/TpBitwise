using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using System.Data;
using System.Net;
using TPBitwiseTraining.DAL.Interfaces;
using TPBitwiseTraining.DTO;
using TPBitwiseTraining.Models;

namespace TPBitwiseTraining.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   // [ResponseCache(CacheProfileName = "Default")]
    public class BrandController : ControllerBase
    {
        private readonly IGenericRepository<Brand> _repository;
        private readonly IBrandRepository _brandRepository;
        private readonly IMapper _mapper;
        protected ResponseApi _responseApi;

        public BrandController(IGenericRepository<Brand> repository, IBrandRepository brandRepository, IMapper mapper)
        {
            _repository = repository;
            _brandRepository = brandRepository;
            _mapper = mapper;
            this._responseApi = new();
        }

        [Authorize(Roles = "user, admin")]
        [OutputCache(Duration = 60, PolicyName = "OutputCacheWithAuthPolicy")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BrandAnswerDTO>>> GetAll()
        {
            var brands = await _repository.GetAll();
            var brandsDto = _mapper.Map<IEnumerable<BrandAnswerDTO>>(brands);
            return Ok(brandsDto);
        }

        [Authorize(Roles = "user, admin")]
        [OutputCache(Duration = 60, PolicyName = "OutputCacheWithAuthPolicy")]
        [HttpGet("{id}")]
        public async Task<ActionResult<BrandAnswerDTO>> GetById(int id)
        {
            var brandDb = await _repository.GetById(id);
            if(brandDb == null)
            {
                _responseApi.StatusCode = HttpStatusCode.NotFound;
                _responseApi.IsSuccess = false;
                _responseApi.ErrorMenssages.Add("The brand is not registrered in data base");
                return NotFound(_responseApi);
            }

            var brandDto = _mapper.Map<BrandAnswerDTO>(brandDb);
            return Ok(brandDto);
        }

        [Authorize(Roles = "user, admin")]
        [OutputCache(Duration = 60, PolicyName = "OutputCacheWithAuthPolicy")]
        [HttpGet("withProducts")]
        public async Task<ActionResult<IEnumerable<BrandAnswerDTO>>> GetBrandWihtProducts()
        {
            var brands = await _brandRepository.GetBrandWihtProducts();
            var brandsDTO = _mapper.Map<IEnumerable<BrandAnswerDTO>>(brands);
            return Ok(brandsDTO);

        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<ActionResult<BrandCreationDTO>> Create(BrandCreationDTO brandCreationDTO)
        {
            var brand = _mapper.Map<Brand>(brandCreationDTO);
            await _repository.Insert(brand);
            var brandDTO = _mapper.Map<BrandAnswerDTO>(brand);
            return CreatedAtAction(nameof(GetAll), new { id = brand.Id }, brandDTO);

        }


        [Authorize(Roles = "admin")]
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

        [Authorize(Roles = "admin")]
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
