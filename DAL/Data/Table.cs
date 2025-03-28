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

    public bool Status { get; set; }

    public DateTime? Createdat { get; set; }

    public DateTime? Modifiedat { get; set; }

    public int Createdby { get; set; }

    public int Modifiedby { get; set; }

    public virtual ICollection<Order> Orders { get; } = new List<Order>();

    public virtual ICollection<Ordertable> Ordertables { get; } = new List<Ordertable>();

    public virtual Section Section { get; set; } = null!;

    public virtual ICollection<Waitingtoken> Waitingtokens { get; } = new List<Waitingtoken>();
}
