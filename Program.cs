using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using SphereWebsite.Business.Interfaces.UserInterface;
using SphereWebsite.Business.Services;
using SphereWebsite.Business.Services.PostsServices;
using SphereWebsite.Data.ApplicationContext;
using SphereWebsite.Data.Interfaces.PostsInterface;
using SphereWebsite.Data.Interfaces.PostsServiceInterface;
using SphereWebsite.Data.Interfaces.UserInterface;
using SphereWebsite.Data.Repositories.UserRepository;
using SphereWebsite.Data.Repositories.PostsRepository; // Certifique-se de incluir a referência ao repositório de post

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPostsService, PostsService>();
builder.Services.AddScoped<IPostRepository, PostsRepository>(); // Altere para usar a classe concreta
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Users/Login";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
