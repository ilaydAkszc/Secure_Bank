using Secure_Bank.Models.Model;

namespace Secure_Bank.Models.ViewModels
{
    public class DashboardViewModel
    {
        public List<Transaction> Transactions { get; set; } = new List<Transaction>();

    }
}
