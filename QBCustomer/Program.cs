using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using QBCustomer.Models;
using QBCustomer.Services;
using QBCustomer.Utils;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace QBCustomer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            AppConfig.Configure();

            builder.Services.AddDbContext<SmartBooksContext>(options => options.UseSqlServer(AppConfig.ConnectionStrings));

            builder.Services.AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<SmartBooksContext>()
            .AddDefaultTokenProviders();

            builder.Services.AddAuthentication(options =>
           {
               options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
               options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
           })
           .AddJwtBearer(options =>
           {
               options.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuer = true,
                   ValidateAudience = true,
                   ValidateLifetime = true,
                   ValidateIssuerSigningKey = true,
                   ValidIssuer = AppConfig.AppSettings["Jwt:Issuer"],
                   ValidAudience = AppConfig.AppSettings["Jwt:Audience"],
                   IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppConfig.AppSettings["Jwt:SecretKey"]))
               };
           });


            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowClient", builder =>
                {
                    builder.WithOrigins("https://localhost:7170")
                           .AllowAnyHeader()
                           .AllowAnyMethod()
                           .AllowCredentials();
                });
            });

            builder.Services.AddControllers();
            builder.Services.AddScoped<QuickBooksService>();
            builder.Services.AddScoped<CustomersService>();
            builder.Services.AddScoped<SbUsersService>();
            builder.Services.AddScoped<AuthService>();
            builder.Services.AddAuthorization();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<SmartBooksContext>();
                dbContext.Database.Migrate();
            }

            app.UseRouting();
            app.UseCors("AllowClient");
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
