using System;
using System.Collections.Generic;

namespace DAL.Data;

public partial class Taxesandfee
{
    public int Taxid { get; set; }

    public string Taxname { get; set; } = null!;

    public int Taxtype { get; set; }

    public bool? Isdeleted { get; set; }

    public bool? Isenabled { get; set; }

    public decimal Taxpercentage { get; set; }

    public bool? Isdefault { get; set; }

    public DateTime? Createdat { get; set; }

    public DateTime? Modifiedat { get; set; }

    public int Createdby { get; set; }

    public int Modifiedby { get; set; }

    public virtual ICollection<Ordertaxesandfee> Ordertaxesandfees { get; set; } = new List<Ordertaxesandfee>();
}
