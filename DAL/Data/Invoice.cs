using System;
using System.Collections.Generic;

namespace DAL.Data;

public partial class Invoice
{
    public int Invoiceid { get; set; }

    public int Orderid { get; set; }

    public int? Orderitemid { get; set; }

    public int? Ordermodifierid { get; set; }

    public DateTime? Createdat { get; set; }

    public DateTime? Modifiedat { get; set; }

    public int Createdby { get; set; }

    public int Modifiedby { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Orderitem? Orderitem { get; set; }

    public virtual Ordermodifier? Ordermodifier { get; set; }

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
