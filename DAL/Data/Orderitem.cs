using System;
using System.Collections.Generic;

namespace DAL.Data;

public partial class Orderitem
{
    public int Orderitemid { get; set; }

    public string Orderitemname { get; set; } = null!;

    public int Itemid { get; set; }

    public int Orderid { get; set; }

    public decimal Orderitemrate { get; set; }

    public int Orderitemquantity { get; set; }

    public decimal? Totalamount { get; set; }

    public bool? Isready { get; set; }

    public string? Specialcomment { get; set; }

    public int Readyitemquanitiy { get; set; }

    public bool? Isdeleted { get; set; }

    public DateTime? Createdat { get; set; }

    public DateTime? Modifiedat { get; set; }

    public int Createdby { get; set; }

    public int Modifiedby { get; set; }

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    public virtual Item Item { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;

    public virtual ICollection<Ordermodifier> Ordermodifiers { get; set; } = new List<Ordermodifier>();

    public virtual ICollection<Ordertaxesandfee> Ordertaxesandfees { get; set; } = new List<Ordertaxesandfee>();
}
