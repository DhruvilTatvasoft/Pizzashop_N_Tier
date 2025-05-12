using System;
using System.Collections.Generic;

namespace DAL.Data;

public partial class ItemModifier
{
    public int Id { get; set; }

    public decimal Total { get; set; }

    public virtual OrderedItem Id1 { get; set; } = null!;

    public virtual Modifier IdNavigation { get; set; } = null!;
}
