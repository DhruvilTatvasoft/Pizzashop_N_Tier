using System;
using System.Collections.Generic;

namespace DAL.Data;

public partial class OrderedItem
{
    public int Id { get; set; }

    public int Quantity { get; set; }

    public virtual Order Id1 { get; set; } = null!;

    public virtual Item IdNavigation { get; set; } = null!;

    public virtual ItemModifier? ItemModifier { get; set; }
}
