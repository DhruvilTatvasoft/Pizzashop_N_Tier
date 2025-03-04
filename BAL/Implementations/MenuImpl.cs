using DAL.Data;

public class MenuImpl : IMenuService
{

     public IGenericRepository _repository;

     public IItemRepository _itemRepository;

     public MenuImpl(IGenericRepository repository,IItemRepository itemRepository){
        _repository = repository;
        _itemRepository = itemRepository;
     }

    public void addNewcategory(MenuModel model,string email)
    {
        string categoryName = model.categoryname;
        string categoryDescription = model.categorydescription;
        _itemRepository.addNewCategory(categoryName,categoryDescription,email);
    }

   

    public MenuModel GetCategories(MenuModel model)
    {
        List<Category> categories = new List<Category>();
        categories = _itemRepository.getAllCategories();
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