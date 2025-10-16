using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity;

namespace SalesWebMVC.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string UserFullName { get; set; }

    }

}

