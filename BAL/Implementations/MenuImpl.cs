using DAL.Data;

public class MenuImpl : IMenuService
{

     public IGenericRepository _repository;

     public MenuImpl(IGenericRepository repository){
        _repository = repository;
     }

    public void addNewcategory(MenuModel model,,string email)
    {
        string categoryName = model.categoryname;
        string categoryDescription = model.categorydescription;
        _repository.addNewCategory(categoryName,categoryDescription,email);
    }

    public MenuModel GetCategories(MenuModel model,string email)
    {
        List<Category> categories = new List<Category>();
        categories = _repository.getAllCategories();
        List<Menu> lst = new List<Menu>();
        foreach(var m in categories){
            Menu menu = new Menu();
            menu.categoryName = m.Categoryname;
            menu.categoryId = m.Categoryid;
            menu.description = m.Description;
            lst.Add(menu);
        }
        model.menuList = lst;
        return model;
    }
}