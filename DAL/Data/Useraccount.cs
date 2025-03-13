using System;
using System.Collections.Generic;

namespace DAL.Data;

public partial class Useraccount
{
    public int Accountid { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int Roleid { get; set; }

    public bool? Isdeleted { get; set; }

    public bool? Isfirstlogin { get; set; }

    public DateTime? Createdat { get; set; }

    public DateTime? Modifiedat { get; set; }

    public int Createdby { get; set; }

    public int Modifiedby { get; set; }

    public virtual Role Role { get; set; } = null!;
}
