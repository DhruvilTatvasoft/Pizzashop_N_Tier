using System.Runtime.CompilerServices;

public class PermissionsModel{
    public List<page> pages{
        get;
        set;
    }
    public int Role{
        get;
        set;
    }   
    
}
public class page{

//  public bool wantsToChange{
//         get;
//         set;
//     }
public int permissionid{
    get;
    set;
}
    public string name{
        get;
        set;
    }
     public bool can_view{
        get;
        set;
    }
    public bool can_Edit{
        get;
        set;
    }
    public bool can_delete{
        get;
        set;
    }
}