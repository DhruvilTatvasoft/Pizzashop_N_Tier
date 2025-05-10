using System;
using System.Collections.Generic;

namespace DAL.Data;

public partial class Ordermodifier
{
    public int Ordermodifierid { get; set; }

    public int Orderitemid { get; set; }

    public string Ordermodifiername { get; set; } = null!;

    public decimal Ordermodifierrate { get; set; }

    public int Modifierid { get; set; }

    public int Ordermodifierquantity { get; set; }

    public decimal Totalamount { get; set; }

    public bool? Isdeleted { get; set; }

    public DateTime? Createdat { get; set; }

    public DateTime? Modifiedat { get; set; }

    public int Createdby { get; set; }

    public int Modifiedby { get; set; }

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    public virtual Modifier Modifier { get; set; } = null!;

    public virtual Orderitem Orderitem { get; set; } = null!;
}
