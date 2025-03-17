using System;
using System.Collections.Generic;

namespace DAL.Data;

public partial class Rolesandpermission
{
    public int Rolesandpermissionid { get; set; }

    public int Permissionid { get; set; }

    public int Roleid { get; set; }

    public bool Canview { get; set; }

    public bool Canedit { get; set; }

    public bool Candelete { get; set; }

    public DateTime? Createdat { get; set; }

    public DateTime? Modifiedat { get; set; }

    public int Createdby { get; set; }

    public int Modifiedby { get; set; }

    public virtual Permission Permission { get; set; } = null!;

    public virtual Role Role { get; set; } = null!;
}
