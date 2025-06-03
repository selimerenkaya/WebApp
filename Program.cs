using ChatForLife.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ChatForLife.Models;
using Microsoft.EntityFrameworkCore;
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
        options.Conventions.AuthorizeFolder("/Profile"); // Tüm Profile klasörü koruma altında
        options.Conventions.AuthorizeFolder("/Chat"); // Tüm Chat klasörü koruma altında
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
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; 
    options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Strict;          
});

// GEÇİCİ OLARAK ASKIYA ALINDI - DÜZENLENECEK
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";              // Giriş yapılmamışsa buraya yönlendir
        options.AccessDeniedPath = "/Account/AccessDenied"; // Yetki yoksa yönlendir
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
        options.SlidingExpiration = true;                  // Süre uzatılır her istekte
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always; //  HTTPS için
        options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Strict;
          //  CSRF önlemi
    });

// ---------------------- JWT Authentication ----------------------
// JWT ayarları
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
builder.Services.AddScoped<IGroupRepository, GroupRepository>();
builder.Services.AddScoped<IGroupMessageRepository, GroupMessageRepository>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
        };

        // TOKEN'I COOKIE'DEN OKU
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var token = context.Request.Cookies["access_token"];
                if (!string.IsNullOrEmpty(token))
                {
                    context.Token = token;
                }
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();




// ---------------------- SWAGGER ----------------------
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.add

var app = builder.Build();

// ---------------------- ENV ----------------------
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();  //
}
else
{
    app.UseExceptionHandler("/Error"); // 
    app.UseHsts();                     // HSTS kullanımı
}


// ---------------------- ORTAK MIDDLEWARE ----------------------
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseStaticFiles();



// ---------------------- ROUTING & SESSION ----------------------
app.UseRouting();

app.UseSession();            // ✅ Session Middleware
app.UseAuthentication(); // JWT kontrolü burada başlar
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
