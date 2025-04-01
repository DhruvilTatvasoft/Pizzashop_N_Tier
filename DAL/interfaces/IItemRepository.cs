using DAL.Data;

public interface IItemRepository {
    List<Item> getItemsForCategory(int categoryId);
    bool addNewCategory(string categoryName, string categoryDescription,string createdBy);

      List<Category> getAllCategories();

      void EditCategory(Menu m,string email);
    void deleteCategory(int categoryId);
    List<Unit> getAllUnits();
    bool addItemInDb(ItemViewModel itemViewModel,string email,string? imagePath);
    bool deleteItemFromDb(int itemId);
    List<Item> getSearchedItemFromDb(string searchedItem,int categoryId);
    List<Modifiergroup> getAllModifierGroups();
    Item getItem(int itemid);
    int getItemFromItemName(string itemname);
}