// IRolePermissionRepository.cs (Interface)
using DAL.Data;

public interface IRoleAndPermissionRepository
{
    void AddPermission(Rolesandpermission permission);
    void UpdatePermission(Rolesandpermission permission);
    void RemovePermission(int permissionId, int roleId);
    Rolesandpermission GetPermission(int permissionId, int roleId);
}
