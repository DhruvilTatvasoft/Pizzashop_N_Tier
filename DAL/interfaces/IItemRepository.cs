using DAL.Data;

public interface IItemRepository {
    List<Item> getItemsForCategory(int categoryId);
    void addNewCategory(string categoryName, string categoryDescription,string createdBy);

      List<Category> getAllCategories();
}