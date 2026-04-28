using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Undervisningsopgaver.Models;

namespace Undervisningsopgaver.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _config;

    public TokenService(IConfiguration config)
    {
        _config = config;
    }

    public string GenerateToken(ApplicationUser user, IList<string> roles)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email!),
            new Claim(ClaimTypes.Name, user.UserName!),
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("FirstName", user.FirstName),
            new Claim("LastName", user.LastName),
            new Claim("Department", "IT"),
            new Claim("BirthDate", "2000-01-01")
        };
            
//         };
//         };            claims.Add(new Claim("BirthDate", "2000-01-01"));

        // Tilføj Department claim, hvis det findes på brugeren
        if (!string.IsNullOrWhiteSpace(user.Department))
        {
            claims.Add(new Claim("Department", user.Department));
        }

        // Tilføj BirthDate claim, hvis det findes på brugeren
        if (user.BirthDate.HasValue)
        {
            claims.Add(new Claim("BirthDate", user.BirthDate.Value.ToString("yyyy-MM-dd")));
        }
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var signingKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var creds = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        var minutes = int.Parse(_config["Jwt:ExpirationInMinutes"] ?? "60");

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(minutes),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
