using System;
using System.Collections.Generic;

namespace DAL.Data;

public partial class Taxis
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public bool? Isenable { get; set; }

    public bool? Default { get; set; }

    public decimal TaxValue { get; set; }
}
