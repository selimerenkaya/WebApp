using ChatForLife.Models;
using ChatForLife.Repositories;
using ChatForLife.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// ---------------------- VERİTABANI ----------------------
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ---------------------- RAZOR PAGES ----------------------
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

// ---------------------- REPO & SERVİSLER ----------------------
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IGroupRepository, GroupRepository>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();
builder.Services.AddScoped<IGroupMessageRepository, GroupMessageRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<IGroupService, GroupService>();

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
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
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
