using System.Net;
using DAL.Data;

public class MenuOrderAppModel
{
    public List<Category> categories{get;set;}
    public List<Item> items{get;set;}

    public CustomerModel customer{get;set;}
    public int orderid{get;set;} = 0;

    public Order order{
        get;set;
    }
    public int readyQuantity{get;set;}
    public string orderComment{get;set;}

    public int itemQuantity{get;set;}

    public string itemcomment{get;set;}
    public List<Taxesandfee> taxesandfees{get;set;}
    public bool isTableAssigned{get;set;} = false;

    public int tokenid{get;set;}
    public int tableid{get;set;}
    public List<ModifierModel> modifiersForItem{get;set;}

    public List<Modifier> modifiers{get;set;}
    public Item item{get;set;}

    public List<string> uniqueids{get;set;}

    public string uniqueId {get;set;}

    public List<Table> tables{get;set;}

    public int categoryId{set;get;} = 0;

    public OrderDetailsViewModel orderDetailModel{get;set;}

    public List<appliedTaxDetails>? appliedTax{get;set;}
}

public class orderDetailsForAssignedTable
{
    public List<int> TableIds { get; set; }
    public int CategoryId { get; set; }
    public string SearchedItem { get; set; }
    public int? orderid { get; set; }
    public int? customerid{get;set;}
}

public class runningTableDetailsViewModel{
   
}


