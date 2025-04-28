using System.Net;
using DAL.Data;

public class MenuOrderAppModel
{
    public List<Category> categories{get;set;}
    public List<Item> items{get;set;}

    public CustomerModel customer{get;set;}
    public int orderid{get;set;}

    public Order order{
        get;set;
    }

    public List<Taxesandfee> taxesandfees{get;set;}
    public bool isTableAssigned{get;set;} 

    public int tokenid{get;set;}
    public int tableid{get;set;}
    public List<ModifierModel> modifiersForItem{get;set;}

    public List<Modifier> modifiers{get;set;}
    public Item item{get;set;}

    public string uniqueId {get;set;}

    public List<Table> tables{get;set;}

    public int categoryId{set;get;} = 0;
}

public class orderDetailsForAssignedTable
{
    public List<int> TableIds { get; set; }
    public int CategoryId { get; set; }
    public string SearchedItem { get; set; }
    public int TokenId { get; set; }
}


