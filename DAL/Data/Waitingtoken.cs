using System;
using System.Collections.Generic;

namespace DAL.Data;

public partial class Waitingtoken
{
    public int Waitingtokenid { get; set; }

    public int Customerid { get; set; }

    public int Totalpersons { get; set; }

    public int Sectionid { get; set; }

    public int Tableid { get; set; }

    public bool? Isdeleted { get; set; }

    public bool? Isassigned { get; set; }

    public DateTime? Createdat { get; set; }

    public DateTime? Modifiedat { get; set; }

    public int Createdby { get; set; }

    public int Modifiedby { get; set; }

    public virtual Section Section { get; set; } = null!;

    public virtual Table Table { get; set; } = null!;
}
