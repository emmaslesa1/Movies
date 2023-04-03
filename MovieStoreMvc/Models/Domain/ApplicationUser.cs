using Microsoft.AspNetCore.Identity;

namespace MoviesMvc.Models.Domain
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
    }
}
