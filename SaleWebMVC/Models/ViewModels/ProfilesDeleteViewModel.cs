using System.ComponentModel.DataAnnotations;

namespace SalesWebMVC.Models.ViewModels
{
    public class ProfilesDeleteViewModel
    {
        public string Id { get; set; }

        [Required]
        [Display(Name = "Nome completo")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Nome completo")]
        public string FullName { get; set; }

        [Required]
        public string Role { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        // Troca de senha
        [DataType(DataType.Password)]
        [Display(Name = "Actual password")]
        public string Password { get; set; }
    }
}
