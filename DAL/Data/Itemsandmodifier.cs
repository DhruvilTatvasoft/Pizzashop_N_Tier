using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Data;

public partial class Itemsandmodifier
{

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Itemsandmodifiersid { get; set; }

    public int Itemid { get; set; }

    public int Modifiergroupid { get; set; }

    public int Allowedmaxselection { get; set; }

    public int Requiredminselection { get; set; }

    public bool? Isdeleted { get; set; }

    public DateTime? Createdat { get; set; }

    public DateTime? Modifiedat { get; set; }

    public int Createdby { get; set; }

    public int Modifiedby { get; set; }

    public virtual Item Item { get; set; } = null!;

    public virtual Modifiergroup Modifiergroup { get; set; } = null!;
}
