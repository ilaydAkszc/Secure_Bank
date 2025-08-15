using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Secure_Bank.Models.DataContext;
using Secure_Bank.Models.Model;
using Secure_Bank.Models.ViewModels;
using System.Security.Claims;

namespace Secure_Bank.Controllers
{

    
        public class BankingController : Controller
        {

            private readonly BankaDbContext _context;

            public BankingController(BankaDbContext context)
            {
                _context = context;
            }
        //KULLANICININ HESAPLARINI GÖSTERME
        public IActionResult ShowAccounts()
            {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "Account");

            var accounts = _context.Accounts.Where(a => a.UserId == userId.Value).ToList();

                return View(accounts);
            }

        //KULLANICIN BİRDEN FAZLA HESABI VARSA HESAPLARIN DETAYLARINI GÖSTERME
       
        public IActionResult AccountDetails(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null || userId <= 0)
                return RedirectToAction("Login", "Account");

            // IDOR:düzeltildi
            var account = _context.Accounts.Include(a => a.User).FirstOrDefault(a => a.AcId == id && a.UserId == userId.Value);
            if (account == null)
            {
                return NotFound(); // veya hata sayfası
            }


            return View(account);
        }
        //GET TRANSFER
        [HttpGet]
            public IActionResult Transfer()
            {
                var userId = HttpContext.Session.GetInt32("UserId");
                if (userId == null || userId <= 0)
                    return RedirectToAction("Login", "Account");

                var accounts = _context.Accounts.Where(a => a.UserId == userId.Value).ToList();

                ViewBag.Accounts = accounts; //Hesap listesini View’a taşımak için
                return View();
            }

           
        
            //POST TRANSFER
            [HttpPost]
            [ValidateAntiForgeryToken] // CSRF ZAFİYETİ - ValidateAntiForgeryToken eklendi                      
            public IActionResult Transfer(TransferViewModel model)
            {
                var userId = HttpContext.Session.GetInt32("UserId");
                if (userId == null || userId <= 0)
                    return RedirectToAction("Login", "Account");


                if (!ModelState.IsValid)
                {
                    var accounts = _context.Accounts.Where(a => a.UserId == userId.Value).ToList();

                    ViewBag.Accounts = accounts; //Hesap listesini View’a taşımak için
                    return View(model);
                }
                //Gönderen hesap kontrolü
                var fromAccount = _context.Accounts.FirstOrDefault(a => a.AcId == model.FromAccountId && a.UserId == userId.Value);
                if (fromAccount == null)
                {
                    ModelState.AddModelError("", "Geçersiz hesap.");
                    var accounts = _context.Accounts.Where(a => a.UserId == userId.Value).ToList();
                    ViewBag.Accounts = accounts;
                    return View(model);
                }
                //Alıcı hesabı bulma

                var toAccount = _context.Accounts
                    .FirstOrDefault(a => a.AccountNumber == model.ToAccountNumber);

                if (toAccount == null)
                {
                    ModelState.AddModelError("ToAccountNumber", "Hedef hesap bulunamadı.");
                    var accounts = _context.Accounts.Where(a => a.UserId == userId.Value).ToList();
                    ViewBag.Accounts = accounts;
                    return View(model);
                }
                 
                //Bakiye kontrolü
                if (fromAccount.Balance < model.Amount)
                {

                    ModelState.AddModelError("Amount", "Bakiye yetersiz.");
                    var accounts = _context.Accounts.Where(a => a.UserId == userId.Value).ToList();
                    ViewBag.Accounts = accounts;
                    return View(model);
                }

                //Gönderen hesaptan para azalt,gönderilene para ekle
                fromAccount.Balance -= model.Amount;
                toAccount.Balance += model.Amount;

                var transaction = new Transaction
                {
                    FromAccountId = fromAccount.AcId,
                    ToAccountId = toAccount.AcId,
                    UserId = userId.Value,
                    Amount = model.Amount,
                    Description = model.Description,
                    TransactionType = "Transfer",
                    TransactionDate = DateTime.Now
                };
                _context.Transactions.Add(transaction);
                _context.SaveChanges();

                ViewBag.Message = "Transfer işlemi başarılı";
                return View();
            }

        //İŞLEM DETAYLARI ---IDOR ZAAFİYETİ
    
            public IActionResult TransactionDetails(int id)
            {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null || userId <= 0)
                return RedirectToAction("Login", "Account");

            // IDOR: düzeltildi
            var transaction = _context.Transactions
          .Include(t => t.FromAccount)
              .ThenInclude(a => a.User)
          .Include(t => t.ToAccount)
              .ThenInclude(a => a.User)
                  .FirstOrDefault(t => t.TrId == id && (t.FromAccount.UserId == userId.Value || t.ToAccount.UserId == userId.Value));
            if (transaction == null)
            {
                return NotFound(); // veya hata sayfası
            }


            return View(transaction);
        }

            //YAPILAN İŞLEMLERİN GÖSTERİLMESİ
            public IActionResult Transactions(int id)
            {
                var userId = HttpContext.Session.GetInt32("UserId");
                if (userId == null || userId <= 0)
                    return RedirectToAction("Login", "Account");

                var userAccounts = _context.Accounts
                                   .Where(a => a.UserId == userId.Value)
                                    .Select(a => a.AcId)
                                     .ToList();
                var transactions = _context.Transactions.Include(t => t.FromAccount).Include(t => t.ToAccount)
                    .Where(t => userAccounts.Contains(t.FromAccountId) || userAccounts.Contains(t.ToAccountId))
                    .OrderByDescending(t => t.TransactionDate).ToList();
                if (transactions == null)
                    return NotFound();


                return View(transactions);
            }


        }
    
}

