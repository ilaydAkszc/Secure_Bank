using System.ComponentModel.DataAnnotations;

namespace Secure_Bank.Models.Model
{
    public class User
    {
        public int UserId { get; set; }
        [Required(ErrorMessage = "Kullanıcı adı gereklidir.")]
        public string Username { get; set; } = string.Empty;
        [Required(ErrorMessage = "Email gereklidir.")]
        [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz.")]
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Şifre gereklidir.")]
        [DataType(DataType.Password)]
        [StringLength(20, ErrorMessage = "Şifre en az {6} ve en fazla {20} karakter olmalıdır.", MinimumLength = 6)]
        public string Password { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();
        public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    }
}
