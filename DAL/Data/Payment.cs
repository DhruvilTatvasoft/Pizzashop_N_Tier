using System;
using System.Collections.Generic;

namespace DAL.Data;

public partial class Payment
{
    public int Paymentid { get; set; }

    public DateTime? Createdat { get; set; }

    public DateTime? Modifiedat { get; set; }

    public int Createdby { get; set; }

    public int Modifiedby { get; set; }

    public char? Paymentmethod { get; set; }

    public char? Status { get; set; }
}
