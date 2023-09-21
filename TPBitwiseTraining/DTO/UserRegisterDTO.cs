using System.ComponentModel.DataAnnotations;

namespace TPBitwiseTraining.DTO
{
    public class UserRegisterDTO
    {
        [Required(ErrorMessage = "You must send the user name")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "You must send the name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        public string Role { get; set; }
    }
}