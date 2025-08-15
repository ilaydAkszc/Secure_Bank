using System.ComponentModel.DataAnnotations;

namespace Secure_Bank.Models.ViewModels
{
    public class TransferViewModel
    {
        [Required]
        [Display(Name = "Gönderen hesap")]
        public int FromAccountId { get; set; }

        [Required]
        [Display(Name = "Alıcı hesap numarası")]
        public string ToAccountNumber { get; set; } = string.Empty;

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Miktar 0 dan büyük olmalıdır")]
        [Display(Name = "Miktar")]
        public decimal Amount { get; set; }

        [Display(Name = "Açıklama")]
        public string Description { get; set; } = string.Empty;
    }
}
