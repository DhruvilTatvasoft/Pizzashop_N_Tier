using DAL.Data;
using Microsoft.Identity.Client;

public class PermissionsModel2{
    public List<Permission> plist{
        get;
        set;
    }
    public List<Rolesandpermission> grantedPermissions{
        get;
        set;
    }
    public int roleid{
        get;
        set;
    }

    public List<int> gpermissionid{
        get;
        set;
    }
    public string rolename{
        get;
        set;
    }
    public List<PermissionModel3> permissionModel{
        get;
        set;
    }
}