using System.ComponentModel.DataAnnotations;

namespace SalesWebMVC.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "{0} is required!")]
        [Display(Name = "user name")]
        public string UserName { get; set; }

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

    }
}
