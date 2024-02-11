using System.Security.Claims;
using ibay.Model;

namespace ibay.Services;

public interface IJwtAuthService
{
    public User Authenticate(string username, string password);

    public string GenerateToken(string secret, List<Claim> claims);
}