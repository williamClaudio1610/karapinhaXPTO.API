using KarapinhaXpto.DTO;
using KarapinhaXpto.Model;
using KarapinhaXpto.Shared.IServices;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace KarapinhaXpto.Services
{
    public class TokenService : ITokenServices
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(Utilizador user)
        {
            var secretKey = _configuration["Jwt:SecretKey"];
            var key = Encoding.ASCII.GetBytes(secretKey);
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim("NomeCompleto", user.NomeCompleto),
                    new Claim("Email", user.Email),
                    new Claim("Telefone", user.Telefone),
                    new Claim("Foto", user.Foto),
                    new Claim("Bi", user.Bi),
                    new Claim("Status", user.Status.ToString()),
                    new Claim(ClaimTypes.Role, user.Role) 

                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
