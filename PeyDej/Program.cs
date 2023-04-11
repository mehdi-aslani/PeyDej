using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using PeyDej;
using PeyDej.Data;
using PeyDej.Models.Users;

using System.Security.Policy;

var builder = WebApplication.CreateBuilder(args);

#if DEBUG
builder.Services.AddHostedService(sp => new NpmWatchHostedService(
    enabled: sp.GetRequiredService<IWebHostEnvironment>().IsDevelopment(),
    logger: sp.GetRequiredService<ILogger<NpmWatchHostedService>>()));
#endif
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession(option => { option.IOTimeout = TimeSpan.FromMinutes(5); }); //you've configured session

builder.Services.AddDbContext<PeyDejContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PeyDejContext_Online") ??
                         throw new InvalidOperationException("Connection string 'PeyDejContext' not found.")));
builder.Services.AddIdentity<PeyDejUser, IdentityRole>(options =>
    {
        // Password settings.
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequiredLength = 6;
        options.Password.RequiredUniqueChars = 0;
        // Lockout settings.
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
        options.Lockout.MaxFailedAccessAttempts = 5;
        options.Lockout.AllowedForNewUsers = true;
        // User settings.
        options.User.AllowedUserNameCharacters =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
        options.User.RequireUniqueEmail = false;
    })
    .AddEntityFrameworkStores<PeyDejContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    // Cookie settings
    options.Cookie.HttpOnly = true;
    options.Cookie.Name = "PeyDej";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    options.LoginPath = "/User/Login";
    options.AccessDeniedPath = "/User/AccessDenied";
    options.SlidingExpiration = true;
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme =
        CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapDefaultControllerRoute();
app.MapAreaControllerRoute("areas", "Admin", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
app.Run();