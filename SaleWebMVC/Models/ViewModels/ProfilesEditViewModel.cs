using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using SalesWebMVC.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace SalesWebMVC.Models.ViewModels
{
    public class ProfilesEditViewModel
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
        public string? CurrentPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Nova senha")]
        public string? NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "As senhas não coincidem.")]
        [Display(Name = "Confirmar nova senha")]
        public string? ConfirmPassword { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Birth Date")]
        public DateTime? BirthDate { get; set; }


        [Display(Name = "Base Salary")]
        [Range(1.0, 5000000.0, ErrorMessage = "{0} must be from {1} to {2}")]
        public double? BaseSalary { get; set; }
    }
}
