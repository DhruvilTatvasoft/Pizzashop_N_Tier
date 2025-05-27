using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
 
public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;
 
    public JwtMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _configuration = configuration;
    }
 
    public async Task Invoke(HttpContext context)
    {
        var path = context.Request.Path.Value?.ToLower();
        if (IsPublicPath(path))
        {
            await _next(context);
            return;
        }
 
        var token = context.Request.Cookies["token"];
        if (!string.IsNullOrEmpty(token))
        {
            var handler = new JwtSecurityTokenHandler();
            var keyVar = _configuration["JWT:SecretKey"];
 
            if (!string.IsNullOrEmpty(keyVar))
            {
                var key = Encoding.UTF8.GetBytes(keyVar);
                try
                {
                    var claimsPrincipal = handler.ValidateToken(token, new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = _configuration["JWT:Issuer"],
                        ValidAudience = _configuration["JWT:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(key)
                    }, out _);
 
                    context.User = claimsPrincipal;
                    await _next(context);
                    return;
                }
                catch
                {
                }
            }
        }
        if (IsProtectedPath(path))
        {
            context.Response.Redirect("/Login/Index");
            return;
        }
        await _next(context);
    }
 
    private bool IsPublicPath(string? path)
    {
        if (string.IsNullOrEmpty(path)) return true;   
        if (path == "/") return true;
        return path.StartsWith("/Login/index")
            || path.StartsWith("/home/accessdenied");
    }
 
    private bool IsProtectedPath(string? path)
    {
        if (string.IsNullOrEmpty(path)) return false;
        return path.StartsWith("/dashboard") || path.StartsWith("/Customer") || path.StartsWith("/Menu") || path.StartsWith("/MenuOrderApp") || path.StartsWith("/TablesAndsections") || path.StartsWith("/Taxes") || path.StartsWith("/Order") || path.StartsWith("/TableView") || path.StartsWith("/WaitingList") || path.StartsWith("/OrderApp") || path.StartsWith("/MenuOrderApp");
    }
}
 
 