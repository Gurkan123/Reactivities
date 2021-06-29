using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace API.Services
{
    public class TokenService
    {
        private readonly IConfiguration _config;
        public TokenService(IConfiguration config)
        {
            _config = config;
        }

        public string CreateToken(AppUser user)
        {
            // var claims = new List<Claim>
            // {
            //     new Claim(ClaimTypes.Name, user.UserName),
            //     new Claim(ClaimTypes.NameIdentifier, user.Id),
            //     new Claim(ClaimTypes.Email, user.Email),
            // };

            var claims = new Claim[]
                {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, user.Id),
            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToUniversalTime().ToString(), ClaimValueTypes.Integer64)
                };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["TokenKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateIssuer = true,
                ValidIssuer = _config["Audience:Iss"],
                ValidateAudience = true,
                ValidAudience = _config["Audience:Aud"],
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                RequireExpirationTime = true,
            };

            var jwt = new JwtSecurityToken(
                issuer: _config["Audience:Iss"],
                audience: _config["Audience:Aud"],
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.Add(TimeSpan.FromHours(2)),
                signingCredentials: creds
            );


            // var tokenDescriptor = new SecurityTokenDescriptor
            // {
            //     // Issuer = "http://localhost:5000/",
            //     // Audience = "https://localhost:44396/",
            //     Subject = new ClaimsIdentity(claims),
            //     Expires = DateTime.UtcNow.AddHours(2),
            //     SigningCredentials = creds
            // };

            var tokenHandler = new JwtSecurityTokenHandler().WriteToken(jwt);
            // var tokenHandler = new JwtSecurityTokenHandler().;


            // var token = tokenHandler.CreateToken(tokenHandler);

            return tokenHandler;
        }

    }
}