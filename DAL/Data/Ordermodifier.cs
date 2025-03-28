using System;
using System.Collections.Generic;

namespace DAL.Data;

public partial class Ordermodifier
{
    public int Ordermodifierid { get; set; }

    public int Modifierid { get; set; }

    public int Ordermodifierquantity { get; set; }

    public bool? Isdeleted { get; set; }

    public DateTime? Createdat { get; set; }

    public DateTime? Modifiedat { get; set; }

    public int Createdby { get; set; }

    public int Modifiedby { get; set; }

    public int Itemid { get; set; }

    public int Orderid { get; set; }

    public int? Orderitemquantity { get; set; }

    public virtual ICollection<Invoice> Invoices { get; } = new List<Invoice>();

    public virtual Item Item { get; set; } = null!;

    public virtual Modifier Modifier { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;
}
