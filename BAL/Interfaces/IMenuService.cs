using DAL.Data;

public interface IMenuService{
    void addNewcategory(MenuModel model);
    public MenuModel GetCategories(MenuModel model);
}