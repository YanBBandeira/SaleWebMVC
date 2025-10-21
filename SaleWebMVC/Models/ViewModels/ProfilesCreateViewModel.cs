using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace SalesWebMVC.Models.ViewModels
{
    public class ProfilesCreateViewModel
    {
        [Required(ErrorMessage = "Name is required!")]
        [Display(Name="Login name")]
        public string Name { get; set; }


        [Display(Name = "User name")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "{0} is required!")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(40, MinimumLength = 6, ErrorMessage = "{0} must be at least {2} and at max {1} characters long.")]
        [DataType(DataType.Password)]
        [Compare("ConfirmPassword", ErrorMessage = "Password does not match.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        public string ConfirmPassword { get; set; }

        public string Role { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Birth Date")]
        public DateTime? BirthDate { get; set; }


        [Display(Name = "Base Salary")]
        [Range(1.0, 5000000.0, ErrorMessage = "{0} must be from {1} to {2}")]
        public double? BaseSalary { get; set; }
    }
}
