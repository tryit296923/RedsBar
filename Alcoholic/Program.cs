using Alcoholic.Models.DTO;
using Alcoholic.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
    //.AddJsonOptions(opt => opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddTransient<MailService>();
builder.Services.AddTransient<HashService>();
builder.Services.AddRazorTemplating();

builder.Services.AddDbContext<db_a8de26_projectContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("remote"));
});

// CORS Middleware�|�B�z���l�ӷ����n�D�C(WithOrigins => �M�Φܫ��w�ӷ�)
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        name: "Policy",
        policy => policy.WithOrigins("http://127.0.0.1:5501", "http://127.0.0.1:5500").WithHeaders("*").WithMethods("*")
    );
});

// �ϥιw�]�� cookie �{��( CookieAuthenticationDefaults )
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(option =>
{
    // ���n�J�۰ʾɦܦ����}
    option.LoginPath = new PathString("/member/LoginRegister");
    option.AccessDeniedPath = new PathString("/api/Login/NoAccess");
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

// UseRouting�� UseAuthorization�e
app.UseCors("Policy");

// �`�N���Ǥ��i�մ�
app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
