using System;
using System.Collections.Generic;

namespace DAL.Data;

public partial class Modifier
{
    public int Modifierid { get; set; }

    public string Modifiername { get; set; } = null!;

    public string? Description { get; set; }

    public bool? Isdeleted { get; set; }

    public int Modifiergroupid { get; set; }

    public decimal Modifierrate { get; set; }

    public int Modifierquantity { get; set; }

    public int Unitid { get; set; }

    public DateTime? Createdat { get; set; }

    public DateTime? Modifiedat { get; set; }

    public int Createdby { get; set; }

    public int Modifiedby { get; set; }

    public virtual Modifiergroup Modifiergroup { get; set; } = null!;

    public virtual ICollection<Ordermodifier> Ordermodifiers { get; set; } = new List<Ordermodifier>();

    public virtual Unit Unit { get; set; } = null!;
}
