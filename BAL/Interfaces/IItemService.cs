using DAL.Data;

public interface IItemService{
    void addItem(Item i, string email);
    bool deleteItem(int itemid);
    void deleteItems(List<int> itemIds);
    Item getItemFromId(int itemid);
    void getItemsForcategory(int categoryId,ItemModel model);
    List<Item> getSearchedItem(string searchedItem,ItemModel model,int categoryId);
}