using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TPBitwiseTraining.DAL.DataContext;
using TPBitwiseTraining.DAL.Interfaces;
using TPBitwiseTraining.DTO;
using TPBitwiseTraining.Models;

namespace TPBitwiseTraining.DAL.Implementations
{
    public class UserRepository : GenericRepository<AppUser>, IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private string claveSecreta;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        public UserRepository(ApplicationDbContext context, IConfiguration config, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper) : base(context)
        {
            _context = context;
            claveSecreta = config.GetValue<string>("ApiSettings:LlaveSecreta");
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }

        public async Task<bool> IsUniqueUser(string usuario)
        {
            var userDB = await _context.Users.FirstOrDefaultAsync(u => u.UserName == usuario);
            if (userDB == null)
                return true;
            return false;
        }

        public async Task<UserAnswerDTO> Login(UserLoginDTO userLoginDTO)
        {
           
            var userDb = await _context.AppUsers.FirstOrDefaultAsync(
                u => u.UserName.ToLower() == userLoginDTO.UserName.ToLower());

            bool isValid = await _userManager.CheckPasswordAsync(userDb, userLoginDTO.Password);

            if (userDb == null || isValid == false)
            {
                return new UserAnswerDTO()
                {
                    Token = "",
                    User = null
                };
            }

            var roles = await _userManager.GetRolesAsync(userDb);
            var manejadorToken = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(claveSecreta);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                        new Claim[]
                        {
                            new Claim(ClaimTypes.Name, userDb.UserName.ToString()),
                            new Claim(ClaimTypes.Role, roles.FirstOrDefault())
                        }
                    ),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = manejadorToken.CreateToken(tokenDescriptor);

            UserAnswerDTO userAnswerDTO = new UserAnswerDTO()
            {
                Token = manejadorToken.WriteToken(token),
                User = _mapper.Map<UserPropertiesDTO>(userDb),
            };

            return userAnswerDTO;

        }

        public async Task<UserPropertiesDTO> Register(UserRegisterDTO userRegisterDTO)
        {



            var userNew = new AppUser()
            {
                UserName = userRegisterDTO.UserName,
                Name = userRegisterDTO.Name,
                Email = userRegisterDTO.UserName,
                NormalizedEmail = userRegisterDTO.UserName.ToUpper()
            };

            var result = await _userManager.CreateAsync(userNew, userRegisterDTO.Password);

            if(result.Succeeded)
            {
                if(!_roleManager.RoleExistsAsync("admin").GetAwaiter().GetResult())
                {
                    await _roleManager.CreateAsync(new IdentityRole("admin"));
                    await _roleManager.CreateAsync(new IdentityRole("user"));
                }

                await _userManager.AddToRoleAsync(userNew, "admin");
                var userReturn = await _context.AppUsers.FirstOrDefaultAsync(u => u.UserName == userRegisterDTO.UserName);

                return _mapper.Map<UserPropertiesDTO>(userReturn);
            }

            return null;
        }

        public async Task<UserDTO> GetById(string id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            var userDTO= _mapper.Map<UserDTO>(user);
            return userDTO;
        }
    }
}
