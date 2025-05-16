using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DAL.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

public class JwtTokenImple : IJwtTokenGenService
{

    private readonly IConfiguration _configuration;
    private readonly string _key;
    private readonly string _issuer;
    private readonly string _audience;

    private readonly IRoleAndPermissionRepository _roleAndPermissionRepository;

    public JwtTokenImple(IConfiguration configuration, IRoleAndPermissionRepository roleAndPermissionRepository)
    {
        _configuration = configuration;
        _key = configuration["Jwt:SecretKey"];
        _issuer = configuration["Jwt:Issuer"];
        _audience = configuration["Jwt:Audience"];
        _roleAndPermissionRepository = roleAndPermissionRepository;
    }

    public string FetchEmail(string Token)
    {
        var tokenEmailClaim = new JwtSecurityTokenHandler().ReadJwtToken(Token).Claims.FirstOrDefault(claim => claim.Type == "mail");
        var tokenEmail = tokenEmailClaim?.Value ?? string.Empty;
        return tokenEmail;
    }

    public string GenerateJwtToken(string userName, string role)
    {
        List<Rolesandpermission> AllPermissions = _roleAndPermissionRepository.GetUserPermissions(role);
        var tokenHandler = new JwtSecurityTokenHandler();

        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:SecretKey"]);

        var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Name, userName),
                new Claim(ClaimTypes.Role, role)
            };
        foreach (var permission in AllPermissions)
        {
            var permissionName = _roleAndPermissionRepository.getPermissionName(permission.Permissionid)?.Trim();
            if (permission != null && !string.IsNullOrWhiteSpace(permissionName))
            {
                claims.Add(new Claim("CanView_" + permissionName, permission.Canview.ToString()));
                claims.Add(new Claim("CanEdit_" + permissionName, permission.Canedit.ToString()));
                claims.Add(new Claim("CanDelete_" + permissionName, permission.Candelete.ToString()));
            }
        }
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            IssuedAt = DateTime.UtcNow,
            Issuer = _issuer,
            Audience = _audience,
            Expires = DateTime.UtcNow.AddHours(5),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
        };


        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public ClaimsPrincipal? ValidateToken(string token)
    {
        if (string.IsNullOrEmpty(token))
            return null;

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_key);

        try
        {
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = _issuer,
                ValidAudience = _audience,
                ClockSkew = TimeSpan.Zero
            };

            var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
            return principal;
        }
        catch
        {
            return null;
        }
    }


}


