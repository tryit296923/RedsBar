using Alcoholic.Hubs;
using Alcoholic.Models;
using Alcoholic.Models.Entities;
using Alcoholic.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddJsonOptions(opt => opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
//builder.Services.AddSingleton<IAuthorizationMiddlewareResultHandler,CookieAuthService>();
builder.Services.AddTransient<MailService>();
builder.Services.AddTransient<HashService>();
builder.Services.AddTransient<AesService>();
builder.Services.AddTransient<LvlService>();
builder.Services.AddRazorTemplating();
builder.Services.AddSignalR();

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

//builder.Services.AddControllers(config =>
//{
//    config.Filters.Add(new ValidationModel());
//});

// 使用預設的 cookie 認證( CookieAuthenticationDefaults )
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(option =>
{
    // 為登入自動導至此網址
    option.LoginPath = new PathString("/Home/oops");
    option.AccessDeniedPath = new PathString("/Home/oops");
}).AddGoogle(opt =>
{
    opt.ClientId = builder.Configuration.GetSection("Auth:Google:ClientId").Value;
    opt.ClientSecret = builder.Configuration.GetSection("Auth:Google:ClientSecret").Value;
    //opt.Events.OnCreatingTicket = context =>
    //{
    //    context.Identity.AddClaim(new System.Security.Claims.Claim(ClaimTypes.Role, "Guest"));
    //    return Task.CompletedTask;
    //};
}).AddTwitter(twitterOptions =>
{
    twitterOptions.ConsumerKey = builder.Configuration.GetSection("Auth:Twitter:ConsumerAPIKey").Value;
    twitterOptions.ConsumerSecret = builder.Configuration.GetSection("Auth:Twitter:ConsumerSecret").Value;
}).AddMicrosoftAccount(microsoftOptions =>
{
    microsoftOptions.ClientId = builder.Configuration.GetSection("Auth:Microsoft:ClientId").Value;
    microsoftOptions.ClientSecret = builder.Configuration.GetSection("Auth:Microsoft:ClientSecret").Value;
}); ;

builder.Services.AddDistributedMemoryCache();

// set up the in-memory session provider with a default in-memory implementation of IDistributedCache
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(10800);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

app.MapHub<NotifyHub>("/notifyHub");

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
app.UseCookiePolicy();
// UseRouting後 UseAuthorization前
app.UseCors("Policy");
app.UseAuthentication();
app.UseAuthorization();


app.MapAreaControllerRoute(
      name: "areas",
      areaName: "BackCenter",
      pattern: "BackCenter/{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
