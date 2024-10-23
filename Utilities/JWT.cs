using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HomeServer.Models;
using Microsoft.IdentityModel.Tokens;

namespace HomeServer.Utilities;

public struct JWT{
    public static string CreateToken(IConfiguration config, PublicUser user){
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(config.GetSection("AppSettings:JWT:Token").Value!);
        var tokenDescriptor = new SecurityTokenDescriptor{
            Subject = new ClaimsIdentity(new Claim[]{
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, "Admin")
            }),
            Expires = DateTime.UtcNow.AddHours(12),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature
            ),
            Issuer = config.GetSection("AppSettings:JWT:Issuer").Value,
            Audience = config.GetSection("AppSettings:JWT:Audience").Value
        };
        
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}