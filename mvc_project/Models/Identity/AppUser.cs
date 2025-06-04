using Microsoft.AspNetCore.Identity;

namespace mvc_project.Models.Identity
{
    public class AppUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int Age { get; set; }
    }
}