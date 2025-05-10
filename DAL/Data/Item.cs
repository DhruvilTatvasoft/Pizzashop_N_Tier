using System;
using System.Collections.Generic;

namespace DAL.Data;

public partial class Item
{
    public int Itemid { get; set; }

    public string Itemname { get; set; } = null!;

    public int Categoryid { get; set; }

    public bool Itemtype { get; set; }

    public decimal Itemrate { get; set; }

    public int Itemquantity { get; set; }

    public bool? Isfavourite { get; set; }

    public bool Isdefaulttax { get; set; }

    public bool Isavailable { get; set; }

    public decimal Taxpercentage { get; set; }

    public string? Shortcode { get; set; }

    public string? Description { get; set; }

    public int Unitid { get; set; }

    public string? Itemimage { get; set; }

    public bool? Isdeleted { get; set; }

    public DateTime? Createdat { get; set; }

    public DateTime? Modifiedat { get; set; }

    public int Createdby { get; set; }

    public int Modifiedby { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<Itemsandmodifier> Itemsandmodifiers { get; set; } = new List<Itemsandmodifier>();

    public virtual ICollection<Orderitem> Orderitems { get; set; } = new List<Orderitem>();

    public virtual Unit Unit { get; set; } = null!;
}
