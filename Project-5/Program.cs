using Microsoft.EntityFrameworkCore;
using Project_5.DAL;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Project_5.Middelwares;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using Project_5.Swagger;
using Microsoft.AspNetCore.Identity;
using Project_5.Models;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwagger>();
builder.Services.AddControllersWithViews();

builder.Services.AddIdentity<AppUser, IdentityRole>(IdentityOption =>
{
    IdentityOption.Password.RequireUppercase = true;
    IdentityOption.Password.RequireLowercase = true;
    IdentityOption.Password.RequireNonAlphanumeric = false;
    IdentityOption.User.RequireUniqueEmail = true;
    IdentityOption.Lockout.AllowedForNewUsers = true;
    IdentityOption.Lockout.MaxFailedAccessAttempts = 5;
    IdentityOption.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
    IdentityOption.SignIn.RequireConfirmedEmail = false;
}).
    AddDefaultTokenProviders().AddEntityFrameworkStores<AppDbContext>();
builder.Services.AddDbContext<AppDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            //ValidIssuer = "example.com",
            //ValidAudience = "example.com",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("My security key with more bits12"))
        };
    });
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://your-swagger-ui-domain.com")
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

// В методе Configure вашего Startup.cs

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
////app.UseEndpoints(endpoints =>
////{
////    endpoints.MapControllerRoute(
////      name: "areas",
////      pattern: "{area:exists}/{controller=Company}"
////    );
////});
//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();
