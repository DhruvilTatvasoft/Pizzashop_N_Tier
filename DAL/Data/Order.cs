using System;
using System.Collections.Generic;

namespace DAL.Data;

public partial class Order
{
    public int Orderid { get; set; }

    public int Customerid { get; set; }

    public int Totalpersons { get; set; }

    public string? Ordercomment { get; set; }

    public int Statusid { get; set; }

    public string Paymentmethod { get; set; } = null!;

    public decimal Subtotalamount { get; set; }

    public int? Taxamount { get; set; }

    public int? Discount { get; set; }

    public decimal Totalamount { get; set; }

    public DateTime? Createdat { get; set; }

    public DateTime? Modifiedat { get; set; }

    public int Createdby { get; set; }

    public int Modifiedby { get; set; }

    public int Rattings { get; set; }

    public bool IsDeleted { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<Invoice> Invoices { get; } = new List<Invoice>();

    public virtual ICollection<Orderitem> Orderitems { get; } = new List<Orderitem>();

    public virtual ICollection<Orderreview> Orderreviews { get; } = new List<Orderreview>();

    public virtual ICollection<Ordertable> Ordertables { get; } = new List<Ordertable>();

    public virtual Orderstatus Status { get; set; } = null!;
}
