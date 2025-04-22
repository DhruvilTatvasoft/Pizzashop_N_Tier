using BAL.Interfaces;
using Microsoft.AspNetCore.Mvc;

public class MenuOrderAppController : Controller
{
    private readonly IMenuOrderAppService _menuOrderAppService;
    private readonly IMenuService _menuService;

    private readonly IWaitingTokenService _waitingTokenService;

    public MenuOrderAppController(IMenuOrderAppService menuOrderAppService,IMenuService menuService,IWaitingTokenService waitingTokenService){
        _menuOrderAppService = menuOrderAppService;
        _menuService = menuService;
        _waitingTokenService = waitingTokenService;
    }
    public IActionResult getMenuSidebar(){
        MenuOrderAppModel model = new MenuOrderAppModel();
        model.categories = _menuService.getAllCategories();
        return PartialView("_menuSidebar",model);
    }
    public IActionResult getItemsForCategory(int categoryid,int tokenid,int tableid,string searchedItem=""){
        MenuOrderAppModel model = new MenuOrderAppModel();
        if(tokenid != 0 && tableid != 0){
            model.customer = _waitingTokenService.getCustomerForWaitingToken(tokenid,tableid);
        }
        model.items = _menuOrderAppService.getItemsForcategory(categoryid,searchedItem);
        model.categoryId = categoryid;
        return PartialView("_menuItems",model);
    }
    public IActionResult getItemDetails(int itemid){
        MenuOrderAppModel model = new MenuOrderAppModel();
        model.modifiersForItem = _menuOrderAppService.getModifiersForItem(itemid);
        model.item = _menuOrderAppService.getItem(itemid);
        return PartialView("_itemDetailsModal",model);
    }

}