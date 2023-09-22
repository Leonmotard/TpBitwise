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
    [ResponseCache(CacheProfileName = "Default")]
    public class CategoryController : ControllerBase
    {
        private readonly IGenericRepository<Category> _repository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        protected ResponseApi _responseApi;

        public CategoryController(IGenericRepository<Category> repository, IMapper mapper, ICategoryRepository categoryRepository)
        {
            _repository = repository;
            _mapper = mapper;
            this._responseApi = new();
            _categoryRepository = categoryRepository;   
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryAnswerDTO>>> GetAll()
        {
            var categories = await _repository.GetAll();
            var categoriesDto = _mapper.Map<IEnumerable<CategoryAnswerDTO>>(categories);
            return Ok(categoriesDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryAnswerDTO>> GetByIdWithProducts(int id)
        {
            var categoryDb = await _categoryRepository.GetByIdWithProducts(id);
            if (categoryDb == null)
            {
                _responseApi.StatusCode = HttpStatusCode.NotFound;
                _responseApi.IsSuccess = false;
                _responseApi.ErrorMenssages.Add("The brand is not registrered in data base");
                return NotFound(_responseApi);
            }

            var categoryDTO = _mapper.Map<CategoryAnswerDTO>(categoryDb);
            return Ok(categoryDTO);
        }


        [HttpPost]
        public async Task<ActionResult<CategoryCreationDTO>> Create(CategoryCreationDTO categoryCreationDTO)
        {
            var category = _mapper.Map<Category>(categoryCreationDTO);
            await _repository.Insert(category);
            var categoryDTO = _mapper.Map<CategoryAnswerDTO>(category);
            return CreatedAtAction(nameof(GetAll), new { id = category.Id }, categoryDTO);

        }



        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var categoryDb = await _repository.GetById(id);

            if (categoryDb == null)
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
        public async Task<ActionResult> Update(int id, CategoryCreationDTO categoryCreationDTO)
        {
            var categoryDb = await _repository.GetById(id);
            if (categoryDb == null)
            {
                _responseApi.StatusCode = HttpStatusCode.NotFound;
                _responseApi.IsSuccess = false;
                _responseApi.ErrorMenssages.Add("The brand is not registrered in data base");
                return NotFound(_responseApi);
            }

            _mapper.Map(categoryCreationDTO, categoryDb);

            var resultado = await _repository.Update(categoryDb);
            if (resultado)
                return NoContent();


            _responseApi.StatusCode = HttpStatusCode.BadRequest;
            _responseApi.IsSuccess = false;
            _responseApi.ErrorMenssages.Add("An error has ocurred");
            return BadRequest(_responseApi);
        }
    }
}

