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
    public class UserController : ControllerBase
    {

        private readonly IGenericRepository<AppUser> _repository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        protected ResponseApi _response;

        public UserController(IGenericRepository<AppUser> repository, IUserRepository userRepository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
            _userRepository = userRepository;
            this._response = new();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAll()
        {
            var users = await _repository.GetAll();
            var usersDto = _mapper.Map<IEnumerable<UserDTO>>(users);
            return Ok(usersDto);
        }

        [HttpGet("{id}", Name = "GetUser")]
        public async Task<ActionResult<UserDTO>> GetById(string id)
        {
            var user = await _userRepository.GetById(id);

            if (user == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.ErrorMenssages.Add("The user is not registrered in data base");
                return NotFound(_response);
            }

            return Ok(user);
        }

            
            [HttpPost("register")]
            public async Task<IActionResult> Register([FromBody] UserRegisterDTO userRegisterDTO)
            {
                var validacionNombre = await _userRepository.IsUniqueUser(userRegisterDTO.UserName);
                if (!validacionNombre)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorMenssages.Add("The user name already exists");
                    return BadRequest(_response);
                }

                var usuario = await _userRepository.Register(userRegisterDTO);
                if (usuario == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorMenssages.Add("The user name already exists");
                    return BadRequest(_response);
                }

                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                return Ok(_response);

            }

            [HttpPost("login")]
            public async Task<IActionResult> login([FromBody] UserLoginDTO userLoginDTO)
            {
                var responseLogin = await _userRepository.Login(userLoginDTO);

                if (responseLogin.User == null || string.IsNullOrEmpty(responseLogin.Token))
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorMenssages.Add("Incorrect user name or password.");
                    return BadRequest(_response);
                }
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Result = responseLogin;
                return Ok(_response);
            }
        
    }
}


