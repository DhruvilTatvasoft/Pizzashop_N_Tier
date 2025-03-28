using System.Collections;
using DAL.Data;

public class orderItemModifierViewModel
{
   public Dictionary<Item, List<Modifier>> modifiersForItem{
        get;
        set;
    }
    public decimal subtotal{get;set;}
}