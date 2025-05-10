using System;
using System.Collections.Generic;

namespace DAL.Data;

public partial class Table
{
    public int Tableid { get; set; }

    public int Sectionid { get; set; }

    public string Tablename { get; set; } = null!;

    public bool? Isdeleted { get; set; }

    public int Capacity { get; set; }

    public int Status { get; set; }

    public DateTime? Createdat { get; set; }

    public DateTime? Modifiedat { get; set; }

    public int Createdby { get; set; }

    public int Modifiedby { get; set; }

    public virtual ICollection<Ordertable> Ordertables { get; set; } = new List<Ordertable>();

    public virtual Section Section { get; set; } = null!;

    public virtual ICollection<Waitingtoken> Waitingtokens { get; set; } = new List<Waitingtoken>();
}
