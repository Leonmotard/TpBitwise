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
    public class ProductController : ControllerBase
    {
        private readonly IGenericRepository<Product> _repository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        protected ResponseApi _responseApi;

        public ProductController(IGenericRepository<Product> repository, IProductRepository productRepository, IMapper mapper)
        {
            _repository = repository;
            _productRepository = productRepository;
            _mapper = mapper;
            this._responseApi = new();
             
        }

        [Authorize(Roles = "user, admin")]
        [OutputCache(Duration = 60, PolicyName = "OutputCacheWithAuthPolicy")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryAnswerDTO>>> GetAll()
        {
            var products = await _productRepository.GetAllAsyncWithData();
            var productsDto = _mapper.Map<IEnumerable<ProductListDTO>>(products);
            return Ok(productsDto);
        }

        [Authorize(Roles = "user, admin")]
        [OutputCache(Duration = 60, PolicyName = "OutputCacheWithAuthPolicy")]
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductAnswerDTO>> Obtener(int id)
        {
            var product = await _productRepository.GetByIdWithData(id);

            if (product == null)
            {
                _responseApi.StatusCode = HttpStatusCode.NotFound;
                _responseApi.IsSuccess = false;
                _responseApi.ErrorMenssages.Add("The product is not registrered in data base");
                return NotFound(_responseApi);
            }

            var productDTO = _mapper.Map<ProductAnswerDTO>(product);

            
            return Ok(productDTO);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<ActionResult<ProductCreationDTO>> Crear(ProductCreationDTO productCreationDTO)
        {
            var product = _mapper.Map<Product>(productCreationDTO);
            await _repository.Insert(product);

            var productDTO = _mapper.Map<ProductAnswerDTO>(product);
            return CreatedAtAction(nameof(GetAll), new { id = product.Id }, productDTO);
        }

        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, ProductCreationDTO productCreationDTO)
        {
            var productDb = await _repository.GetById(id);
            if (productDb == null)
            {
                _responseApi.StatusCode = HttpStatusCode.NotFound;
                _responseApi.IsSuccess = false;
                _responseApi.ErrorMenssages.Add("The product is not registrered in data base");
                return NotFound(_responseApi);
            }

            _mapper.Map(productCreationDTO, productDb);

            var resultado = await _repository.Update(productDb);
            if (resultado)
                return NoContent();


            _responseApi.StatusCode = HttpStatusCode.BadRequest;
            _responseApi.IsSuccess = false;
            _responseApi.ErrorMenssages.Add("An error has ocurred");
            return BadRequest(_responseApi);
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var productDb = await _repository.GetById(id);

            if (productDb == null)
            {
                _responseApi.StatusCode = HttpStatusCode.NotFound;
                _responseApi.IsSuccess = false;
                _responseApi.ErrorMenssages.Add("The product is not registrered in data base");
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

    }
}
