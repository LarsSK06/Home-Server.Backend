using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HomeServer.Models;
using Microsoft.IdentityModel.Tokens;

namespace HomeServer.Utilities;

public struct JWT{
    public static string CreateToken(IConfiguration config, PublicUser user){
        List<Claim> claims = new List<Claim>{
            new Claim(ClaimTypes.Email, user.Email)
        };

        SymmetricSecurityKey key =
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                config.GetSection("AppSettings:Token").Value!)
            );

        SigningCredentials credentials =
            new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        JwtSecurityToken token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddHours(12),
            signingCredentials: credentials
        );

        string jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }
}