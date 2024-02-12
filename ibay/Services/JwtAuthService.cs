using System.IdentityModel.Tokens.Jwt;
using System.Runtime.InteropServices.JavaScript;
using System.Security.Claims;
using System.Text;
using ibay.Model;
using Microsoft.IdentityModel.Tokens;

namespace ibay.Services;

public class JwtAuthService : IJwtAuthService
{
    private IbayContext ibayContext;
    
    public JwtAuthService()
    {
        this.ibayContext = new IbayContext();
        this.ibayContext.Database.EnsureCreated();
        
    }

    public User Authenticate(string username, string password)
    {
        return this.ibayContext.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
    }

    public string GenerateToken(string secret, List<Claim> claims)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(1),
            SigningCredentials = new SigningCredentials(
                key, 
                SecurityAlgorithms.HmacSha256)
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}