using ChatForLife.Models;
using ChatForLife.Repositories;
using ChatForLife.Services;
<<<<<<< HEAD
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// ---------------------- VERİTABANI ----------------------
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ---------------------- RAZOR PAGES ----------------------
=======
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers; 
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// Razor sayfalarını servise ekler
>>>>>>> developer
builder.Services.AddRazorPages()
    .AddRazorPagesOptions(options =>
    {
        options.Conventions.AddPageRoute("/Account/Login", "giris");
        options.Conventions.AddPageRoute("/Account/Register", "kayit");
        options.Conventions.AuthorizeFolder("/Dashboard"); // Tüm Dashboard klasörü koruma altında
        options.Conventions.AuthorizeFolder("/Profile"); // Tüm Dashboard klasörü koruma altında
        options.Conventions.AuthorizeFolder("/Chat"); // Tüm Dashboard klasörü koruma altında
        options.Conventions.AllowAnonymousToPage("/Account/Login");
        options.Conventions.AllowAnonymousToPage("/Account/Register");
    });

<<<<<<< HEAD
// ---------------------- REPO & SERVİSLER ----------------------
=======
>>>>>>> developer
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IGroupRepository, GroupRepository>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();
builder.Services.AddScoped<IGroupMessageRepository, GroupMessageRepository>();
<<<<<<< HEAD
=======

>>>>>>> developer
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<IGroupService, GroupService>();

<<<<<<< HEAD
// ---------------------- SESSION & COOKIE ----------------------
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Oturum süresi
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";            // Giriş yapılmamışsa buraya yönlendir
        options.AccessDeniedPath = "/Account/AccessDenied"; // Yetki yoksa yönlendir
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
        options.SlidingExpiration = true; // Süre uzatılır her istekte
    });

// ---------------------- ANTIFORGERY ----------------------
//builder.Services.AddAntiforgery(options =>
//{
//    options.Cookie.HttpOnly = true;
//    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
//});

// ---------------------- SWAGGER ----------------------
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ---------------------- ENV ----------------------
=======
// Anti-forgery ve güvenli cookie ayarları
builder.Services.AddAntiforgery(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});
// API için gerekli servisler
builder.Services.AddControllers(); 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();  


var app = builder.Build();



>>>>>>> developer
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
<<<<<<< HEAD
}

// ---------------------- ORTAK MIDDLEWARE ----------------------
app.UseHttpsRedirection();
app.UseStaticFiles();

// Güvenlik başlıkları
//app.Use(async (context, next) =>
//{
//    context.Response.Headers["X-Frame-Options"] = "DENY";
//    context.Response.Headers["X-XSS-Protection"] = "1; mode=block";
//    context.Response.Headers["Content-Security-Policy"] = "default-src 'self'; script-src 'self'; style-src 'self'";
//    await next();
//});

// ---------------------- ROUTING & SESSION ----------------------
app.UseRouting();

app.UseSession();            // ✅ Session Middleware
app.UseAuthentication();     // ✅ Cookie Authentication
app.UseAuthorization();      // Yetki kontrolü

// ---------------------- RAZOR & API ----------------------
app.MapRazorPages();
app.MapControllers();

// ---------------------- SWAGGER ----------------------
app.UseSwagger();
app.UseSwaggerUI();

// ---------------------- DB Migration ----------------------
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.Run();
=======
}
app.UseHttpsRedirection();       // HTTP → HTTPS yönlendirme  


app.UseStaticFiles();   

app.UseHttpsRedirection();       // HTTP → HTTPS yönlendirme    
app.UseStaticFiles();            // wwwroot klasöründen statik dosya sunumu
// Güvenlik başlıkları : Clickjacking, XSS ve CSP koruması bi  dk commitleri bkmadım

app.Use(async (context, next) =>
{ // Bu sayfanın iframe içinde açılmasını engeller.
    context.Response.Headers["X-Frame-Options"] = "DENY"; 
    // Tarayıcıya XSS koruması talimatı verir
    context.Response.Headers["X-XSS-Protection"] = "1; mode=block";
      // İçerik sadece kendi sunucundan yükler
    context.Response.Headers["Content-Security-Policy"] = "default-src 'self'; script-src 'self'; style-src 'self'";
    await next();
});

         // wwwroot klasöründen statik dosya sunumu
app.UseRouting();                // Route işlemleri

app.UseAuthorization();          // Yetkilendirme kontrolü
app.MapRazorPages();      
// http://localhost:5228/swagger ile kontrol edebilir
app.UseSwagger();
app.UseSwaggerUI(); 
app.MapControllers(); 

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate(); // 🧠 Bu satır EF migration'ları otomatik çalıştırır
}

app.Run();                       // Uygulamayı başlatır
      // Razor Pages'i route'a bağlar

>>>>>>> developer
