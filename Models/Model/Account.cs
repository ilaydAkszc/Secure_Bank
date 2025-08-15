using System.ComponentModel.DataAnnotations;

namespace Secure_Bank.Models.Model
{
    public class Account
    {
        public int AcId { get; set; }
        [Required]
        public int UserId { get; set; }

        [Required]
        public string AccountNumber { get; set; } = string.Empty;

        public decimal Balance { get; set; }

        public string AccountType { get; set; } = "Vadesiz"; // Vadeli,Tasarruf

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation property
        public virtual User User { get; set; } = null!;
        public virtual ICollection<Transaction> FromTransactions { get; set; } = new List<Transaction>();
        public virtual ICollection<Transaction> ToTransactions { get; set; } = new List<Transaction>();
    }
}
