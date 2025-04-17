using Microsoft.AspNetCore.Mvc;

public class MenuOrderAppController : Controller
{
    private readonly IMenuOrderAppService _menuOrderAppService;
    private readonly IMenuService _menuService;

    public MenuOrderAppController(IMenuOrderAppService menuOrderAppService,IMenuService menuService){
        _menuOrderAppService = menuOrderAppService;
        _menuService = menuService;
    }
    public IActionResult getMenuSidebar(){
        MenuOrderAppModel model = new MenuOrderAppModel();
        model.categories = _menuService.getAllCategories();
        return PartialView("_menuSidebar",model);
    }
    public IActionResult getItemsForCategory(int categoryid){
        MenuOrderAppModel model = new MenuOrderAppModel();
        model.items = _menuOrderAppService.getItemsForcategory(categoryid);
        return PartialView("_menuItems",model);
    }

}