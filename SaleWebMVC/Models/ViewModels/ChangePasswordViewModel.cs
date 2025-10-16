using System.ComponentModel.DataAnnotations;

namespace SalesWebMVC.Models.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Name is required")] 
        public string UserName { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(40, MinimumLength = 6, ErrorMessage = "{0} must be at least {2} and at max {1} characters long.")]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        [Compare("ConfirmNewPassword", ErrorMessage = "Password does not match.")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        public string ConfirmNewPassword { get; set; }
    }
}
