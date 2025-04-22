using DAL.Data;

public class MenuOrderAppModel
{
    public List<Category> categories{get;set;}
    public List<Item> items{get;set;}

    public CustomerModel customer{get;set;}

    public bool isTableAssigned{get;set;} 

    public int tokenid{get;set;}
    public int tableid{get;set;}
    public List<ModifierModel> modifiersForItem{get;set;}
    public Item item{get;set;}

    public int categoryId{set;get;}
}

