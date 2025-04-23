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
            model.tokenid = tokenid;
            model.isTableAssigned = true;
        }
        model.items = _menuOrderAppService.getItemsForcategory(categoryid,searchedItem);
        model.categoryId = categoryid;
        return PartialView("_itemData",model);
    }
    public IActionResult getMenuDataContainer(){
        MenuOrderAppModel model = new MenuOrderAppModel();
        model.categories = _menuService.getAllCategories();
        return PartialView("_menuItems",model);
    }
    public IActionResult getItemDetails(int itemid,string isTableAssigned = "False"){
        MenuOrderAppModel model = new MenuOrderAppModel();
        model.modifiersForItem = _menuOrderAppService.getModifiersForItem(itemid);
        model.item = _menuOrderAppService.getItem(itemid);
        if(isTableAssigned == "True"){
            model.isTableAssigned = true;
        }
        else{
            model.isTableAssigned = false;
        }
        return PartialView("_itemDetailsModal",model);
    }

     public IActionResult showCustomerDetails(int tokenid)
    {
        MenuOrderAppModel model = new MenuOrderAppModel();
        model.customer = _waitingTokenService.getCustomerForWaitingToken(tokenid, 0);
        return PartialView("_customerDetailModal", model);
    }
[HttpPost]
    public IActionResult editCustomerDetails(MenuOrderAppModel model){
        _menuOrderAppService.saveCustomerDetails(model.customer);
        return Json(new {success = "Customer Details Save successfully"});
    }

}