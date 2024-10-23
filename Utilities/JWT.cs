using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HomeServer.Models;
using Microsoft.Extensions.Primitives;
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

    public static JwtSecurityToken? ReadToken(HttpRequest request){
        if(!request.Headers.TryGetValue("Authorization", out StringValues header))
            return null;
        
        JwtSecurityTokenHandler handler = new();
        SecurityToken? securityToken = handler.ReadToken(header.ToString().Split(" ")[1]);

        if(securityToken is not null){
            try{ return securityToken as JwtSecurityToken; }
            catch{ return null; }
        }
        else return null;
    }

    public static int? GetTokenUserId(JwtSecurityToken? token){
        if(token is null)
            return null;

        if(!int.TryParse(
            token.Claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.NameId)?.Value,
            out int parse
        )) return null;

        return parse;
    }

    public static bool CompareUserId(JwtSecurityToken? token, int userId){
        return GetTokenUserId(token) == userId;
    }
}