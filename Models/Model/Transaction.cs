using System.ComponentModel.DataAnnotations;

namespace Secure_Bank.Models.Model
{
    public class Transaction
    {
        public int TrId { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public int FromAccountId { get; set; }
        [Required]
        public int ToAccountId { get; set; }
        [Required]
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; } = DateTime.Now;
        public string Description { get; set; } = string.Empty;

        public string TransactionType { get; set; } = "Transfer"; // Transfer, Para Çekme, Para Yatırma
        // Navigation properties
        public virtual Account FromAccount { get; set; } = null!; // bu değer şu anda null gibi gözüküyor ama runtime’da null olmayacak
        public virtual Account ToAccount { get; set; } = null!;
        public virtual User User { get; set; } = null!;

    }
}
