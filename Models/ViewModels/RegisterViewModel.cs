using System.ComponentModel.DataAnnotations;

namespace Secure_Bank.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Kullanıcı adı")]
        public string Username { get; set; } = string.Empty;

        [Required]
        [Display(Name = "İsim Soyisim")]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Şifre")]
        public string Password { get; set; } = string.Empty;

        [Required]
        [Compare("Password")]
        [Display(Name = "Şifre doğrulama")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
