using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Secure_Bank.Models.DataContext;
using Secure_Bank.Models.Model;
using Secure_Bank.Models.ViewModels;
using System.Security.Cryptography;
using System.Text;

namespace Secure_Bank.Controllers
{
    public class AccountController : Controller
    {
        private readonly BankaDbContext _context;
        // PasswordHasher, ASP.NET Core Identity tarafından sağlanan bir sınıftır.
        private readonly PasswordHasher<User> _passwordHasher = new PasswordHasher<User>();


        public AccountController(BankaDbContext context)
        {
            _context = context;
        }

        //LOGİN
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
      
        public async Task<IActionResult> LoginAsync(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Kullanıcıyı sadece kullanıcı adına göre bul.
            var user = _context.Users.FirstOrDefault(u => u.Username == model.Username);

            // Kullanıcı ve şifre kontrolü
            if (user != null)
            {
                var passwordResult = _passwordHasher.VerifyHashedPassword(user, user.Password, model.Password);

                if (passwordResult == PasswordVerificationResult.Success)
                {
                    // Eski session'ı temizle
                    HttpContext.Session.Clear();

                    // Yeni session oluştur
                    await HttpContext.Session.CommitAsync();

                    // Session'a kullanıcı bilgilerini kaydet
                    HttpContext.Session.SetString("Username", user.Username);
                    HttpContext.Session.SetInt32("UserId", user.UserId);
                    HttpContext.Session.SetString("FullName", user.FullName);
                    HttpContext.Session.SetString("Email", user.Email);

                    return RedirectToAction("Dashboard", "Home");
                }
            }

            // Kullanıcı bulunamadıysa VEYA şifre yanlışsa genel hata mesajını göster.

            ModelState.AddModelError("", "Geçersiz kullanıcı adı veya şifre.");
            return View(model);
        }
        //LOGOUT
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        //REGİSTER
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            
            // Kullanıcı adı daha önce alınmış mı kontrolü 
            var existUser = _context.Users.FirstOrDefault(u => u.Username == model.Username);

            if (existUser != null)
            {
                ModelState.AddModelError("Username", "Bu kullanıcı adı kullanılıyor.");
                return View(model);
            }

         


            // Yeni kullanıcı oluştur 
            var user = new User
            {
                Username = model.Username,
                FullName = model.FullName,
                Email = model.Email,
                CreatedDate = DateTime.Now
            };

            // Şifreyi hashleme
            user.Password = _passwordHasher.HashPassword(user, model.Password);

            _context.Users.Add(user);
            _context.SaveChanges();

            //Yeni kullanıcı için account id oluşturma
            // En büyük mevcut hesap numarasını bulma
            int lastAccountNumber = _context.Accounts
                .OrderByDescending(a => a.AccountNumber)
                .Select(a => Convert.ToInt32(a.AccountNumber))
                .FirstOrDefault();

            string newAccountNumber = (lastAccountNumber + 1).ToString();

            // Varsayılan hesap oluştur
            var account = new Models.Model.Account
            {
                UserId = user.UserId,
                AccountNumber = newAccountNumber,
                Balance = 100.00m, // Hoş geldin bonusu
                AccountType = "Vadesiz",
                CreatedDate = DateTime.Now
            };

            _context.Accounts.Add(account);
            _context.SaveChanges();

            return RedirectToAction("Login");
        }

        //PROFILE GÜNCELLEME

        [HttpGet]
        public IActionResult ProfilEdit()
        {
            var username = HttpContext.Session.GetString("Username");
            if (username == null)
                return RedirectToAction("Login");

            var user = _context.Users.FirstOrDefault(u => u.Username == username);
            if (user == null)
                return RedirectToAction("Login");

            var model = new ProfilViewModel
            {
                UserId = user.UserId,
                Username = user.Username,
                FullName = user.FullName,
                Email = user.Email,
                Bio = user.Bio
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ProfilEdit(ProfilViewModel model)
        {
            var username = HttpContext.Session.GetString("Username");
            if (username == null)
                return RedirectToAction("Login");

            var user = _context.Users.FirstOrDefault(u => u.Username == username);
            if (user == null)
                return RedirectToAction("Login");

            // XSS'ye karşı  temizlik
            user.FullName = System.Net.WebUtility.HtmlEncode(model.FullName);
            user.Email = System.Net.WebUtility.HtmlEncode(model.Email);
            user.Bio = System.Net.WebUtility.HtmlEncode(model.Bio);

            _context.SaveChanges();

            ViewBag.Message = "Profil başarıyla güncellendi!";
            return View(model);
        }


     
    }
}
