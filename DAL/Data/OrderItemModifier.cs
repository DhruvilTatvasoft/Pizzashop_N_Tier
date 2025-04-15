using System;
using System.Collections.Generic;

namespace DAL.Data;

public partial class OrderItemModifier
{
    public int Orderitemmodifierid { get; set; }

    public int? ItemId { get; set; }

    public int? Modifierid { get; set; }

    public int? Orderitemdetailid { get; set; }

    public int? Orderid { get; set; }

    public bool? IsReady { get; set; }

    public int? ModifierQuantity { get; set; }

    public virtual Item? Item { get; set; }

    public virtual Modifier? Modifier { get; set; }

    public virtual Order? Order { get; set; }

    public virtual Orderitem? Orderitemdetail { get; set; }
}
