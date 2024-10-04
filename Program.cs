using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using SphereWebsite.Business.Interfaces.CommentsInterface;
using SphereWebsite.Business.Interfaces.S3Interface;
using SphereWebsite.Business.Interfaces.UserInterface;
using SphereWebsite.Business.Services;
using SphereWebsite.Business.Services.AWS;
using SphereWebsite.Business.Services.CommentsServices;
using SphereWebsite.Business.Services.PostsServices;
using SphereWebsite.Data.ApplicationContext;
using SphereWebsite.Data.Interfaces.CommentsInterface;
using SphereWebsite.Data.Interfaces.PostsInterface;
using SphereWebsite.Data.Interfaces.PostsServiceInterface;
using SphereWebsite.Data.Interfaces.UserInterface;
using SphereWebsite.Data.Repositories.CommentsRepository;
using SphereWebsite.Data.Repositories.PostsRepository;
using SphereWebsite.Data.Repositories.UserRepository;
using SphereWebSite.Business.Interfaces.GroupInterface;
using SphereWebSite.Business.Services.GroupService;
using SphereWebSite.Data.Interfaces.GroupRepository;
using SphereWebSite.Data.Repositories.GroupRepository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPostsService, PostsService>();
builder.Services.AddScoped<IPostRepository, PostsRepository>();
builder.Services.AddScoped<ICommentsService, CommentsService>();
builder.Services.AddScoped<ICommentsRepository, CommentRepository>();
builder.Services.AddScoped<IS3Service, S3Service>();
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<S3Service>();
builder.Services.AddScoped<IGroupService, GroupsService>();
builder.Services.AddScoped<IGroupRepository, GroupRepository>();


builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder
    .Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Users/Login";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    });

builder.Services.AddHttpsRedirection(options =>
{
    options.RedirectStatusCode = StatusCodes.Status308PermanentRedirect;
    options.HttpsPort = 7145;
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
