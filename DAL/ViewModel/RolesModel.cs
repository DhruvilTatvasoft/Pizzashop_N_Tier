using DAL.Data;

public class RolesModel{
    public int roleid{
        get;
        set;
    }
    public string rolename{
        get;
        set;
    }
  
    public List<Role> Role{
        get;
        set;
    }
}