// PermissionService.cs (Implementation)
using DAL.Data;

public class PermissionImple : IPermissionService
{
    private readonly IRoleAndPermissionRepository _repository;

    public PermissionImple(IRoleAndPermissionRepository repository)
    {
        _repository = repository;
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
