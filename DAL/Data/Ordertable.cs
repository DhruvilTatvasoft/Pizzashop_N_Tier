using System;
using System.Collections.Generic;

namespace DAL.Data;

public partial class Ordertable
{
    public int Ordertableid { get; set; }

    public int Orderid { get; set; }

    public int Tableid { get; set; }

    public int Totalpersonsoftable { get; set; }

    public bool? Isdeleted { get; set; }

    public DateTime? Createdat { get; set; }

    public DateTime? Modifiedat { get; set; }

    public int Createdby { get; set; }

    public int Modifiedby { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Table Table { get; set; } = null!;
}
