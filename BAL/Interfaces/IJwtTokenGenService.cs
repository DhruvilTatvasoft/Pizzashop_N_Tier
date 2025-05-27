using System.Security.Claims;

public interface IJwtTokenGenService
{
    string GenerateJwtToken(string userName, string role);
    ClaimsPrincipal? ValidateToken(string token);
}