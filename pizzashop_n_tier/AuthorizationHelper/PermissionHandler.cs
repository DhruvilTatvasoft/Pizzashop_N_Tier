using Microsoft.AspNetCore.Authorization;

using System.Security.Claims;


public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly IAuthServices _authServices;
    private readonly IPermissionService _rolesAndPermissionServices;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public PermissionHandler(IAuthServices authServices,IPermissionService  rolesAndPermissionServices,IHttpContextAccessor httpContextAccessor)
    {
        _authServices = authServices;
        _rolesAndPermissionServices = rolesAndPermissionServices;
        _httpContextAccessor = httpContextAccessor;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        var Email = _httpContextAccessor.HttpContext.Request.Cookies["username"];
        var roleName =  _authServices.GetUserRole(Email);
        if (string.IsNullOrEmpty(roleName))
        {
            context.Fail();
            return;
        }
        var RoleNameForPermission = roleName + "_CanViewOrderApp";
        var permissions = _rolesAndPermissionServices.GetPermissionForAuthorization(roleName);
        // var roles = _rolesAndPermissionServices.GetRolesForOrderApp();
        // if (permissions.Contains(requirement.Permission) && roles.Contains(RoleNameForPermission))
        if (permissions.Contains(requirement.Permission))
        {
            context.Succeed(requirement);
        }
        else
        {
            context.Fail();
        }
    }
}