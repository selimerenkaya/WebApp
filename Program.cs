var builder = WebApplication.CreateBuilder(args);

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

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapRazorPages(); 
app.MapControllers(); 
//controller route a ekleme


app.UseSwagger();
app.UseSwaggerUI(); 
 // http://localhost:5228/swagger ile kontrol edebilir

app.Run(); 
