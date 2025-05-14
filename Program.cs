var builder = WebApplication.CreateBuilder(args);
// Razor sayfalarını servise ekler
builder.Services.AddRazorPages()
    .AddRazorPagesOptions(options =>
    {
        options.Conventions.AddPageRoute("/Account/Login", "giris");
        options.Conventions.AddPageRoute("/Account/Register", "kayit");
    });

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts(); // HTTPS güvenliği için
}

app.UseHttpsRedirection();       // HTTP → HTTPS yönlendirme
app.UseStaticFiles();            // wwwroot klasöründen statik dosya sunumu
app.UseRouting();                // Route işlemleri
app.UseAuthorization();          // Yetkilendirme kontrolü
app.MapRazorPages();             // Razor Pages'i route'a bağlar
app.Run();                       // Uygulamayı başlatır
