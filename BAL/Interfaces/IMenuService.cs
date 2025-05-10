using System.Runtime.CompilerServices;
using DAL.Data;

public interface IMenuService{
    bool addNewcategory(MenuModel model,string email);
    public MenuModel GetCategories(MenuModel model);

    void editCategory(MenuModel model,string email);
    void deleteCategory(int categoryId);
}