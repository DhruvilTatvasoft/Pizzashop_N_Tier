using System.Collections;
using DAL.Data;

public class orderItemModifierViewModel
{
    public Dictionary<Item, List<Modifier>> modifiersForItem
    {
        get;
        set;
    }   = new Dictionary<Item, List<Modifier>>();

    public Dictionary<int, Dictionary<Item, List<Modifier>>> ItemsAndModifiers
    {
        get; set;
    }   = new Dictionary<int, Dictionary<Item, List<Modifier>>>();
    public decimal subtotal { get; set; }
}

public class OrderItemViewModelProc
{
    public int order_id { get; set; }
    public int item_id { get; set; }

   public  string item_name { get; set; }   = string.Empty;
   public  int orderquantity { get; set; }
   public  string itemcomment { get; set; } = string.Empty;

   public  int readyquantity { get; set; }
   public  int orderitemdetailid { get; set; }
   public  int? modifier_id { get; set; }
   public  string? modifier_name { get; set; }
   public  string unique_id { get; set; }   = string.Empty;
}

public class ItemViewModelForOrder
{
    public int itemid { get; set; }
    public int itemquantity { get; set; }
    public string itemName { get; set; }  = string.Empty;
}