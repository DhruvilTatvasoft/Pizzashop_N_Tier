using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;

public class CustomAuthorizationMiddlewareResultHandler : IAuthorizationMiddlewareResultHandler
{
     private readonly AuthorizationMiddlewareResultHandler _defaultHandler = new AuthorizationMiddlewareResultHandler();

     public async Task HandleAsync(
       RequestDelegate next,
       HttpContext context,
       AuthorizationPolicy policy,
       PolicyAuthorizationResult authorizeResult)
    {
        if (authorizeResult.Forbidden)
        {
            context.Items["PermissionDenied"] = true;   
            await next(context);
            return;
        }
        await _defaultHandler.HandleAsync(next, context, policy, authorizeResult);
    }
}