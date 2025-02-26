using System;
using System.Collections.Generic;

namespace DAL.Data;

public partial class Orderreview
{
    public int Reviewid { get; set; }

    public int Orderid { get; set; }

    public int? Foodreview { get; set; }

    public int? Servicereview { get; set; }

    public int? Ambiencereview { get; set; }

    public string? Comment { get; set; }

    public int? Averagerating { get; set; }

    public DateTime? Createdat { get; set; }

    public DateTime? Modifiedat { get; set; }

    public int Createdby { get; set; }

    public int Modifiedby { get; set; }

    public virtual Order Order { get; set; } = null!;
}
