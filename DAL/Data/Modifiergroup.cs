using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace DAL.Data;

public partial class Modifiergroup
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Modifiergroupid { get; set; }

    public string Modifiergroupname { get; set; } = null!;

    public string? Description { get; set; }

    public bool? Isdeleted { get; set; }

    public DateTime? Createdat { get; set; }

    public DateTime? Modifiedat { get; set; }

    public int Createdby { get; set; }

    public int Modifiedby { get; set; }

    public virtual ICollection<Itemsandmodifier> Itemsandmodifiers { get; set; } = new List<Itemsandmodifier>();

    public virtual ICollection<Modifier> Modifiers { get; set; } = new List<Modifier>();
}
