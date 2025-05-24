using ChatForLife.Models;
using ChatForLife.Repositories;
using ChatForLife.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers; 
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// Razor sayfalarını servise ekler
builder.Services.AddRazorPages()
    .AddRazorPagesOptions(options =>
    {
        options.Conventions.AddPageRoute("/Account/Login", "giris");
        options.Conventions.AddPageRoute("/Account/Register", "kayit");
    });

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IGroupRepository, GroupRepository>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();
builder.Services.AddScoped<IGroupMessageRepository, GroupMessageRepository>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<IGroupService, GroupService>();

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



if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
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


using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate(); // 🧠 Bu satır EF migration'ları otomatik çalıştırır
}

app.Run();                       // Uygulamayı başlatır
      // Razor Pages'i route'a bağlar
app.MapControllers(); 
