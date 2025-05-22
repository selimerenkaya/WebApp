using ChatForLife.Models;
using Microsoft.EntityFrameworkCore;

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
app.UseStaticFiles();            // wwwroot klasöründen statik dosya sunumu
app.UseRouting();                // Route işlemleri
app.UseAuthorization();          // Yetkilendirme kontrolü
app.MapRazorPages();             // Razor Pages'i route'a bağlar
app.MapControllers(); 
// http://localhost:5228/swagger ile kontrol edebilir
app.UseSwagger();
app.UseSwaggerUI(); 


using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate(); // 🧠 Bu satır EF migration'ları otomatik çalıştırır
}

app.Run();                       // Uygulamayı başlatır
