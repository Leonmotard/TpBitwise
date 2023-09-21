using Microsoft.AspNetCore.Identity;

namespace TPBitwiseTraining.Models
{
    public class AppUser : IdentityUser
    {
        public string Name { get; set; }
    }
}
