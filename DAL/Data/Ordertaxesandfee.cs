using System;
using System.Collections.Generic;

namespace DAL.Data;

public partial class Ordertaxesandfee
{
    public int Ordertaxid { get; set; }

    public int Orderitemid { get; set; }

    public int Taxid { get; set; }

    public string Ordertaxname { get; set; } = null!;

    public decimal Ordertaxamount { get; set; }

    public bool? Isdeleted { get; set; }

    public DateTime? Createdat { get; set; }

    public DateTime? Modifiedat { get; set; }

    public int Createdby { get; set; }

    public int Modifiedby { get; set; }

    public virtual Orderitem Orderitem { get; set; } = null!;

    public virtual Taxesandfee Tax { get; set; } = null!;
}
