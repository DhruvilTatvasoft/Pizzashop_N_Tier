// IPermissionService.cs (Interface)
public interface IPermissionService
{
    void UpdatePermission(int permissionId, bool canView, bool canEdit, bool canDelete, int roleId);
    void UpdatePermissions(PermissionsModel2 model);
}
