using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace EventBookingSystem.Application.JWT
{
    public class JwtAuthService(IConfiguration configuration) : IJwtAuthService
    {
        private readonly IConfiguration _configuration = configuration;

        public string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            string pem = _configuration["Jwt:secretKey"];

            byte[] pkcs1Bytes = Convert.FromBase64String(pem);

            var rsa = RSA.Create();
            rsa.ImportRSAPrivateKey(pkcs1Bytes, out _);

            var credentials = new SigningCredentials(new RsaSecurityKey(rsa), SecurityAlgorithms.RsaSha256);
            var tokeOptions = new JwtSecurityToken(
                 issuer: _configuration["Jwt:Issuer"],
                 audience: _configuration["Jwt:Audience"],
                 claims: claims,
                 expires: DateTime.Now.AddMinutes(120),
                 signingCredentials: credentials
             );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
            return tokenString;
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}
