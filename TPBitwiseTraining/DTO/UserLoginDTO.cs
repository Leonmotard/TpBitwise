﻿using System.ComponentModel.DataAnnotations;

namespace TPBitwiseTraining.DTO
{
    public class UserLoginDTO
    {
        [Required(ErrorMessage = "User name is required")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
