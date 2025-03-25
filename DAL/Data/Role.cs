using System;
using System.Collections.Generic;

namespace DAL.Data;

public partial class Role
{
    public int Roleid { get; set; }

    public string Rolename { get; set; } = null!;

    public DateTime? Createdat { get; set; }

    public DateTime? Modifiedat { get; set; }

    public int Createdby { get; set; }

    public int Modifiedby { get; set; }

    public virtual ICollection<Rolesandpermission> Rolesandpermissions { get; } = new List<Rolesandpermission>();

    public virtual ICollection<Useraccount> Useraccounts { get; } = new List<Useraccount>();

    public virtual ICollection<User> Users { get; } = new List<User>();
}
