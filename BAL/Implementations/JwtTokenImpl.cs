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





    // public async Task<string> GenerateJWT(User user)
    // {
    //     var AllPermissions = await _rolesAndPermissionsService.GetUserPermissions(user.Id);
    //     Role Role = await _rolesAndPermissionsService.GetRoleById(user.RoleId);
    //     var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
    //     var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
    //     var userClaims = new List<Claim>
    //     {
    //         new Claim("userName",user.Username),
    //         new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
    //         new Claim("mail",user.Email),
    //         new Claim(ClaimTypes.Role,Role.Rolename),
    //     };
    //     foreach (var permission in AllPermissions)
    //     {
    //         var requiredPermission = await _rolesAndPermissionsService.GetPermissionById(permission.PermissionId);
    //         if (requiredPermission != null && requiredPermission.Permissionname != null)
    //         {
    //             userClaims.Add(new Claim("CanView_" + requiredPermission.Permissionname, permission.CanView.ToString()));
    //             userClaims.Add(new Claim("CanEdit_" + requiredPermission.Permissionname, permission.CanEdit.ToString()));
    //             userClaims.Add(new Claim("CanDelete_" + requiredPermission.Permissionname, permission.CanDelete.ToString()));
    //         }
    //     }

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


