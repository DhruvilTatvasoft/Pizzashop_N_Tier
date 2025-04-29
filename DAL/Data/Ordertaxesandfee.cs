using System;
using System.Collections.Generic;

namespace DAL.Data;

public partial class Ordertaxesandfee
{
    public int Ordertaxid { get; set; }

    public int? Orderid { get; set; }

    public string? Taxtype { get; set; }

    public decimal? TaxPercentage { get; set; }

    public string? Taxname { get; set; }

    public virtual Order? Order { get; set; }
}
