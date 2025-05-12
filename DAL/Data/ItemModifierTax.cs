using System;
using System.Collections.Generic;

namespace DAL.Data;

public partial class ItemModifierTax
{
    public int Id { get; set; }

    public string PaymentStatus { get; set; } = null!;

    public DateOnly? OrderDate { get; set; }

    public string Status { get; set; } = null!;

    public string PaymentMethod { get; set; } = null!;

    public virtual Saction Id1 { get; set; } = null!;

    public virtual TableDetail Id2 { get; set; } = null!;

    public virtual Customer IdNavigation { get; set; } = null!;

    public virtual Order? Order { get; set; }
}
