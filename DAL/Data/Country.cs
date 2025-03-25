using System;
using System.Collections.Generic;

namespace DAL.Data;

public partial class Country
{
    public int Countryid { get; set; }

    public string Countryname { get; set; } = null!;

    public DateTime? Createdat { get; set; }

    public DateTime? Modifiedat { get; set; }

    public int Createdby { get; set; }

    public int Modifiedby { get; set; }

    public virtual ICollection<State> States { get; } = new List<State>();

    public virtual ICollection<User> Users { get; } = new List<User>();
}
