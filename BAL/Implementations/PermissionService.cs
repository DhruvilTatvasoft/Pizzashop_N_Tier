

using DAL.Data;

public class PermissionService : IPermissionService
{
    private readonly IRoleAndPermissionRepository _repository;

    public PermissionService(IRoleAndPermissionRepository repository)
    {
        _repository = repository;
    }

    public List<string> GetAllPolicies()
    {
        var allPermissions =  _repository.getAllPermissions();
 
        List<string> policies = new List<string>();
 
        foreach (var perm in allPermissions)
        {
            policies.Add($"{perm.Permissionname}_CanView");
            policies.Add($"{perm.Permissionname}_CanEditAdd");
            policies.Add($"{perm.Permissionname}_CanDelete");
        }
        return policies.Distinct().ToList();
    }

    public List<string> GetPermissionForAuthorization(string roleName)
    {
       
        var role = _repository.GetRoleByRoleName(roleName);
        if (role == null) return new List<string>();

        var rolePermissions = _repository.GetRolePermissions(role.Roleid);

        List<string> permissionPolicies = new List<string>();

        foreach (var perm in rolePermissions)
        {
            if (perm.Canview) permissionPolicies.Add($"{perm.Permission.Permissionname}_CanView");
            if (perm.Canedit) permissionPolicies.Add($"{perm.Permission.Permissionname}_CanEditAdd");
            if (perm.Candelete) permissionPolicies.Add($"{perm.Permission.Permissionname}_CanDelete");
        }
        return permissionPolicies;
    }


    public void UpdatePermission(int permissionId, bool canView, bool canEdit, bool canDelete, int roleId)
    {
        var existingPermission = _repository.GetPermission(permissionId, roleId);

        if (existingPermission == null)
        {
            var newPermission = new Rolesandpermission
            {
                Roleid = roleId,
                Permissionid = permissionId,
                Canview = canView,
                Canedit = canEdit,
                Candelete = canDelete
            };

            _repository.AddPermission(newPermission);
        }
        else
        {
            existingPermission.Canview = canView;
            existingPermission.Canedit = canEdit;
            existingPermission.Candelete = canDelete;
            _repository.UpdatePermission(existingPermission);
        }
    }

    public void UpdatePermissions(PermissionsModel2 model)
    {
        foreach (var permission in model.permissionModel)
        {
            if (permission.IsChecked)
            {
                UpdatePermission(permission.PermissionId, permission.can_view, permission.can_edit, permission.can_delete, model.roleid);
            }
            else
            {
                _repository.RemovePermission(permission.PermissionId, model.roleid);
            }
        }
    }
}
