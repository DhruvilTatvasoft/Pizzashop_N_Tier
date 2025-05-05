using System.Collections;
using DAL.Data;

public class orderItemModifierViewModel 
{
   public Dictionary<Item, List<Modifier>> modifiersForItem{
        get;
        set;
    }

    public Dictionary<int,Dictionary<Item,List<Modifier>>> ItemsAndModifiers{
        get;set;
    }
    public decimal subtotal{get;set;}
}

public class ItemViewModelForOrder{
    public int itemid{get;set;}
    public int itemquantity {get;set;}
    public string itemName{get;set;}
}