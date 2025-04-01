using DAL.Data;

public interface IItemService{
    bool addItem(ItemViewModel itemViewModel, string email);
    bool deleteItem(int itemid);
    void deleteItems(List<int> itemIds);
    List<Category> getAllCategories();
    List<Modifiergroup> getAllModifierGroups();
    List<Unit> getAllUnits();
    Item getItemFromId(int itemid);
    int getItemFromItemName(string itemname);
    void getItemsForcategory(int categoryId,ItemModel model);
    
    List<Item> getSearchedItem(string searchedItem,ItemModel model,int categoryId);
}