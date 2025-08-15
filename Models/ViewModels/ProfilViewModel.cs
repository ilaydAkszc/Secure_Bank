using System.ComponentModel.DataAnnotations;

namespace Secure_Bank.Models.ViewModels
{
    public class ProfilViewModel
    {
        public int UserId { get; set; }

        [Required]
        [Display(Name = "Kullanıcı Adı")]
        public string Username { get; set; } = string.Empty;

        [Required]
        [Display(Name = "İsim Soyisim")]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Hakkımda")]
        public string Bio { get; set; } = string.Empty;
    }
}
