using System;
using System.Collections.Generic;

namespace DAL.Data;

public partial class Payment
{
    public int Paymentid { get; set; }

    public int Invoiceid { get; set; }

    public decimal Amount { get; set; }

    public int Paymentmethod { get; set; }

    public int Status { get; set; }

    public DateTime? Createdat { get; set; }

    public DateTime? Modifiedat { get; set; }

    public int Createdby { get; set; }

    public int Modifiedby { get; set; }

    public virtual Invoice Invoice { get; set; } = null!;
}
