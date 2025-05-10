using System;
using System.Collections.Generic;

namespace DAL.Data;

public partial class Unit
{
    public int Unitid { get; set; }

    public string Unitname { get; set; } = null!;

    public string Shortname { get; set; } = null!;

    public bool? Isdeleted { get; set; }

    public DateTime? Createdat { get; set; }

    public DateTime? Modifiedat { get; set; }

    public int Createdby { get; set; }

    public int Modifiedby { get; set; }

    public virtual ICollection<Item> Items { get; set; } = new List<Item>();

    public virtual ICollection<Modifier> Modifiers { get; set; } = new List<Modifier>();
}
