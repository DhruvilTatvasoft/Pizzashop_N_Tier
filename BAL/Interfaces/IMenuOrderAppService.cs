using DAL.Data;

public interface IMenuOrderAppService
{
    Item getItem(int itemid);
    List<Item> getItemsForcategory(int categoryid,string searchedItem);
    List<ModifierModel> getModifiersForItem(int itemid);
}