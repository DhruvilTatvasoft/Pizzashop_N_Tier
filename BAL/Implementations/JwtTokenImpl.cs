using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

public class JwtTokenImple : IJwtTokenGenService{

     private readonly IConfiguration _configuration;

     public JwtTokenImple(IConfiguration configuration){
        _configuration = configuration;
     }
    public string GenerateJwtToken(string userName, string role)
{
    var tokenHandler = new JwtSecurityTokenHandler();
    var key = Encoding.ASCII.GetBytes(_configuration["JWT:SecretKey"]);

    var claims = new List<Claim>
    {
         new Claim(JwtRegisteredClaimNames.Name, userName), // Set Subject claim for Username
        new Claim(ClaimTypes.Role, role)   
    };

    var tokenDescriptor = new SecurityTokenDescriptor
    {
        Subject = new ClaimsIdentity(claims),
        IssuedAt = DateTime.UtcNow,
        Issuer = _configuration["JWT:Issuer"],
        Audience = _configuration["JWT:Audience"],
        Expires = DateTime.UtcNow.AddMinutes(150),
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
    };

    var token = tokenHandler.CreateToken(tokenDescriptor);
    return tokenHandler.WriteToken(token);
}

}