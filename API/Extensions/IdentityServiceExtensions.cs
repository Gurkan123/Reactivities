using System;
using System.Text;
using API.Services;
using Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Persistence;

namespace API.Extensions
{
    public static class IdentityServiceExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddIdentityCore<AppUser>(opt => {
                opt.Password.RequireNonAlphanumeric = false;
            })
            .AddEntityFrameworkStores<DataContext>()
            .AddSignInManager<SignInManager<AppUser>>();

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt => 
                {
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = key,
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
            services.AddScoped<TokenService>();

            // var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
            // var tokenValidationParameters = new TokenValidationParameters
            // {
            //     ValidateIssuerSigningKey = true,
            //     IssuerSigningKey = signingKey,
            //     ValidateIssuer = true,
            //     ValidIssuer = config["Audience:Iss"],
            //     ValidateAudience = true,
            //     ValidAudience = config["Audience:Aud"],
            //     ValidateLifetime = true,
            //     ClockSkew = TimeSpan.Zero,
            //     RequireExpirationTime = true,
            // };

            // services.AddAuthentication(o =>
            // {
            //     o.DefaultAuthenticateScheme = "TestKey";
            // });

            // services.AddAuthentication()
            //         .AddJwtBearer("TestKey", x =>
            //          {
            //              x.RequireHttpsMetadata = false;
            //              x.TokenValidationParameters = tokenValidationParameters;
            //          });

            return services;
        }
    }
}