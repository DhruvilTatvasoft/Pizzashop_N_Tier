using DAL.Data;

public class MenuOrderAppModel
{
    public List<Category> categories{get;set;}
    public List<Item> items{get;set;}

    public CustomerModel customer{get;set;}
}