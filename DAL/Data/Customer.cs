using System;
using System.Collections.Generic;

namespace DAL.Data;

public partial class Customer
{
    public int Customerid { get; set; }

    public string Customername { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Phonenumber { get; set; } = null!;

    public DateTime? Createdat { get; set; }

    public DateTime? Modifiedat { get; set; }

    public int Createdby { get; set; }

    public int Modifiedby { get; set; }

    public virtual ICollection<Order> Orders { get; } = new List<Order>();
}
