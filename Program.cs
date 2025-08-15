using Microsoft.EntityFrameworkCore;
using Secure_Bank.Models.DataContext;

var builder = WebApplication.CreateBuilder(args);


//***********
// Connection string'i appsettings.json dosyasından al
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// DbContext'i servislere ekle
builder.Services.AddDbContext<BankaDbContext>(options =>
    options.UseSqlite(connectionString));
//***********

// Session ekleniyor
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true; //Cookie’ye JavaScript ile erişilemez (document.cookie engellenir).
    options.Cookie.IsEssential = true; //GDPR / KVKK gereği, bu cookie kullanıcı izni olmadan da oluşturulabilir (çünkü uygulama için zorunlu).
    //eklemeler 
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; //Cookie sadece HTTPS üzerinden gönderilir.
    options.Cookie.SameSite = SameSiteMode.Strict;//Cookie sadece aynı domain’den gelen isteklerde gönderilir.→ CSRF saldırılarını zorlaştırır.
    options.Cookie.Name = "__SecureSession";// Cookie'nin adını belirler, bu adla tarayıcıda saklanır.__Secure- ile başlayan cookie isimleri tarayıcıda güvenlik açısından bir standarttır
});



// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Session middleware aktif ediliyor
app.UseSession();

//Authentication ve Authorization middleware'leri ekleniyor
app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
