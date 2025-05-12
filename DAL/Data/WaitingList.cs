using System;
using System.Collections.Generic;

namespace DAL.Data;

public partial class WaitingList
{
    public int Id { get; set; }

    public DateTime? CreatedTime { get; set; }

    public DateTime? WaitingTime { get; set; }

    public string Name { get; set; } = null!;

    public int PersonCount { get; set; }

    public int Phone { get; set; }

    public string Email { get; set; } = null!;

    public bool? Isdeleted { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public int CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public int? DeletedBy { get; set; }

    public virtual Saction Id1 { get; set; } = null!;

    public virtual Customer IdNavigation { get; set; } = null!;
}
