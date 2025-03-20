using System;
using System.Collections.Generic;

namespace DAL.Data;

public partial class Modifiergroup
{
    public int Modifiergroupid { get; set; }

    public string Modifiergroupname { get; set; } = null!;

    public string? Description { get; set; }

    public bool? Isdeleted { get; set; }

    public DateTime? Createdat { get; set; }

    public DateTime? Modifiedat { get; set; }

    public int Createdby { get; set; }

    public int Modifiedby { get; set; }

    public virtual ICollection<Itemsandmodifier> Itemsandmodifiers { get; set; } = new List<Itemsandmodifier>();

    public virtual ICollection<Modifier> Modifiers { get; set; } = new List<Modifier>();
}
