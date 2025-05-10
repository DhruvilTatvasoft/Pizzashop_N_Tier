using System;
using System.Collections.Generic;

namespace DAL.Data;

public partial class Permission
{
    public int Permissionid { get; set; }

    public string Permissionname { get; set; } = null!;

    public DateTime? Createdat { get; set; }

    public DateTime? Modifiedat { get; set; }

    public int Createdby { get; set; }

    public int Modifiedby { get; set; }

    public virtual ICollection<Rolesandpermission> Rolesandpermissions { get; set; } = new List<Rolesandpermission>();
}
