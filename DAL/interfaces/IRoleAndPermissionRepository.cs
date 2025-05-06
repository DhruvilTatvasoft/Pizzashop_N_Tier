using DAL.Data;

public interface IRoleAndPermissionRepository
{
    void AddPermission(Rolesandpermission permission);
    void UpdatePermission(Rolesandpermission permission);
    void RemovePermission(int permissionId, int roleId);
    Rolesandpermission GetPermission(int permissionId, int roleId);
    List<Rolesandpermission> GetUserPermissions(string role);
    string getPermissionName(int permissionid);
    List<Permission> getAllPermissions();
    Role GetRoleByRoleName(string roleName);
    List<Rolesandpermission> GetRolePermissions(int roleid);
}
