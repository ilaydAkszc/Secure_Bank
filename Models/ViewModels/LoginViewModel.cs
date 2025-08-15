using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;

namespace Secure_Bank.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Kullanıcı adı")]
        public string Username { get; set; } = string.Empty;
        [Required]
        [Display(Name = "Şifre")]
        public string Password { get; set; } = string.Empty;
        public string ReturnUrl { get; set; } = string.Empty; //kullanıcının yönlendirileceği hedef sayfa URL'si





    }
}
