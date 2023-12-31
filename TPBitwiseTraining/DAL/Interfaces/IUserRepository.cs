﻿using TPBitwiseTraining.DTO;
using TPBitwiseTraining.Models;

namespace TPBitwiseTraining.DAL.Interfaces
{
    public interface IUserRepository : IGenericRepository<AppUser>
    {
        Task<bool> IsUniqueUser(string usuario);
        Task<UserPropertiesDTO> RegisterAdmin(UserRegisterDTO userRegisterDTO);
        Task<UserPropertiesDTO> RegisterUser(UserRegisterDTO userRegisterDTO);
        Task<UserAnswerDTO> Login(UserLoginDTO userLoginDTO);
        Task<UserDTO> GetById(string id);
    }
}
