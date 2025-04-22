using DAL.Data;

public interface IMenuOrderAppRepository
{
    Item getItem(int itemid);
    List<Item> getItemsForcategory(int categoryid,string searchedItem);
    List<ModifierModel> getModifiersForItem(int itemid);
}