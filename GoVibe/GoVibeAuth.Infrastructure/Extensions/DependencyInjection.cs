using System.Text;
using GoVibeAuth.Domain.Entities;
using GoVibeAuth.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace GoVibeAuth.Infrastructure.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // services.AddDbContext<AppDbContext>(options =>
            // {
            //     options.UseNpgsql(configuration.GetConnectionString("PostgresConnectionStrings"));
            // });

            services.AddDbContextPool<AppDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("PostgresConnectionString")));
        
            services.AddPooledDbContextFactory<AppDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("PostgresConnectionString")));
            
            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<AppDbContext>() // provide our context
                .AddDefaultTokenProviders() // create email for email confirmation
                .AddRoles<ApplicationRole>() // be able to add roles
                .AddRoleManager<RoleManager<ApplicationRole>>() // be able to make use of RoleManager
                .AddSignInManager<SignInManager<ApplicationUser>>() // make use of sign in manager
                .AddUserManager<UserManager<ApplicationUser>>(); // make use of user manager to create user
            
            services.AddAuthentication()
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = true;

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateIssuerSigningKey = true,
                        ValidateAudience = false,
                        ValidIssuer = configuration["JWT:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(configuration["JWT:Key"]))
                    };
                });
            
            return services;
        }
    }
}
