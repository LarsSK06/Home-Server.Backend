using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HomeServer.Models;
using Microsoft.IdentityModel.Tokens;

namespace HomeServer.Utilities;

public struct JWT{
    public static string CreateToken(IConfiguration config, User user){
        JwtSecurityTokenHandler? tokenHandler = new();

        byte[]? key = Encoding.ASCII.GetBytes(config.GetSection("AppSettings:JWT:Token").Value!);

        SecurityTokenDescriptor? tokenDescriptor = new(){
            Subject = new ClaimsIdentity(new Claim[]{
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Role,
                    user.Admin
                        ? "Admin"
                        : "Regular"
                )
            }),
            Expires = DateTime.UtcNow.AddHours(12),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature
            ),
            Issuer = config.GetSection("AppSettings:JWT:Issuer").Value,
            Audience = config.GetSection("AppSettings:JWT:Audience").Value
        };
        
        SecurityToken? token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}