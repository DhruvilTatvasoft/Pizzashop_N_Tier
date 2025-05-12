using System;
using System.Collections.Generic;

namespace DAL.Data;

public partial class TableDetail
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int Capacity { get; set; }

    public bool Status { get; set; }

    public bool? Isdeleted { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public int? DeletedBy { get; set; }

    public virtual Saction IdNavigation { get; set; } = null!;

    public virtual ItemModifierTax? ItemModifierTax { get; set; }

    public virtual Order? Order { get; set; }
}
