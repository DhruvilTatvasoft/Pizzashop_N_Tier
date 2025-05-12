using System;
using System.Collections.Generic;

namespace DAL.Data;

public partial class Saction
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public virtual ItemModifierTax? ItemModifierTax { get; set; }

    public virtual Order? Order { get; set; }

    public virtual TableDetail? TableDetail { get; set; }

    public virtual WaitingList? WaitingList { get; set; }
}
