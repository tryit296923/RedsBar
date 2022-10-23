using Alcoholic.Models.Entities;
using Alcoholic.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddJsonOptions(opt => opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddTransient<MailService>();
builder.Services.AddTransient<HashService>();
builder.Services.AddRazorTemplating();

builder.Services.AddDbContext<db_a8de26_projectContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("remote"));
    option.UseLazyLoadingProxies();
});

// CORS Middleware會處理跨原始來源的要求。(WithOrigins => 套用至指定來源)
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        name: "Policy",
        policy => policy.WithOrigins("http://127.0.0.1:5501", "http://127.0.0.1:5500").WithHeaders("*").WithMethods("*")
    );
});

// 使用預設的 cookie 認證( CookieAuthenticationDefaults )
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(option =>
{
    // 為登入自動導至此網址
    option.LoginPath = new PathString("/member/LoginRegister");
    option.AccessDeniedPath = new PathString("/api/Login/NoAccess");
}).AddGoogle(opt =>
{
    opt.ClientId = builder.Configuration.GetSection("Auth:Google:ClientId").Value;
    opt.ClientSecret = builder.Configuration.GetSection("Auth:Google:ClientSecret").Value;
    opt.Events.OnCreatingTicket = context =>
    {
        context.Identity.AddClaim(new System.Security.Claims.Claim(ClaimTypes.Role, "Guest"));
        return Task.CompletedTask;
    };
});

builder.Services.AddDistributedMemoryCache();

// set up the in-memory session provider with a default in-memory implementation of IDistributedCache
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(10800);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

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
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();
// UseRouting後 UseAuthorization前
app.UseCors("Policy");

// 注意順序不可調換
//app.UseCookiePolicy();


app.MapAreaControllerRoute(
      name: "areas",
      areaName: "BackCenter",
      pattern: "BackCenter/{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
