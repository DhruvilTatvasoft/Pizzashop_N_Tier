using BAL.Interfaces;
using DAL.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
// using Microsoft.EntityFrameworkCore.Metadata.Internal;

public class MenuOrderAppController : Controller
{
    private readonly IMenuOrderAppService _menuOrderAppService;
    private readonly IMenuService _menuService;

    private readonly IWaitingTokenService _waitingTokenService;
    private readonly IOrderService _orderService;

    private readonly IItemService _itemService;
    private readonly IModifierService _modifierService;
    private readonly ITableService _tableService;
    private readonly ITaxService _taxesService;


    public MenuOrderAppController(IMenuOrderAppService menuOrderAppService,ITaxService taxesService,ITableService tableService,IItemService itemService,IModifierService modifierService,IMenuService menuService,IOrderService orderService,IWaitingTokenService waitingTokenService){
        _menuOrderAppService = menuOrderAppService;
        _menuService = menuService;
        _waitingTokenService = waitingTokenService;
        _orderService = orderService;
        _itemService = itemService;
        _modifierService = modifierService;
        _tableService = tableService;
        _taxesService = taxesService;
    }
    public IActionResult getMenuSidebar(){
        MenuOrderAppModel model = new MenuOrderAppModel();
        model.categories = _menuService.getAllCategories();
        return PartialView("_menuSidebar",model);
    }

[HttpPost]

public IActionResult getItemsForCategory([FromBody]orderDetailsForAssignedTable model)
{
    MenuOrderAppModel responseModel = new MenuOrderAppModel();

    if (model.TokenId != 0)
    {
        List<int> tableIds = new List<int>();
        if(model.TableIds.Count == 0)
        {
        tableIds = new List<int> { 0 }; 
        }
        else{
            tableIds = model.TableIds;
            
        }
        responseModel.customer = _waitingTokenService.getCustomerForWaitingToken(model.TokenId, tableIds);
        responseModel.tokenid = model.TokenId;
        responseModel.isTableAssigned = true;

        List<DAL.Data.Table> tables = new List<DAL.Data.Table>();
        foreach (int tableId in model.TableIds)
        {
            DAL.Data.Table table = _tableService.gettablebyid(tableId);
            tables.Add(table);
        }
        responseModel.tables = tables;
    }

    if (model.SearchedItem == null)
    {
        model.SearchedItem = "";
    }

    responseModel.items = _menuOrderAppService.getItemsForcategory(model.CategoryId, model.SearchedItem);
    responseModel.categoryId = model.CategoryId;

    return PartialView("_itemData", responseModel);
}
[HttpPost]
public IActionResult getOrderDetails([FromBody]assignTableDetails model){
    MenuOrderAppModel responseModel = new MenuOrderAppModel();
    responseModel.customer = _waitingTokenService.getCustomerForWaitingToken(model.tokenid, model.tableids);
    responseModel.tables = new List<DAL.Data.Table>();
    foreach(int tableid in model.tableids){
        DAL.Data.Table table = _tableService.gettablebyid(tableid);
        responseModel.tables.Add(table);
    }
    responseModel.taxesandfees = _taxesService.getAllTaxes();
    responseModel.tokenid = model.tokenid;

    return PartialView("_orderDetailModal",responseModel);
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
        List<int> tableid = new List<int>{0};
        model.customer = _waitingTokenService.getCustomerForWaitingToken(tokenid, tableid);
        return PartialView("_customerDetailModal", model);
    }
[HttpPost]
    public IActionResult editCustomerDetails(MenuOrderAppModel model){
        _menuOrderAppService.saveCustomerDetails(model.customer);
        return Json(new {success = "Customer Details Save successfully"});
    }


[HttpPost]
    public IActionResult addItemInOrder(int itemid,List<int> modifiers,string uniqueId){
        MenuOrderAppModel model = new MenuOrderAppModel();
        model.item = _itemService.getItemFromId(itemid);
        List<Modifier> modifierList = new List<Modifier>();
        foreach(int modifierid in modifiers){
            Modifier modifier = _modifierService.getModifierFromId(modifierid);
            modifierList.Add(modifier);
        }
        model.modifiers = modifierList;
        model.uniqueId = uniqueId;
        return PartialView("_itemAccordian",model);
    }

    [HttpPost]
    public IActionResult saveTheOrderDetails([FromBody] OrderDetailsViewModel orderDetails){
        _menuOrderAppService.createOrder(orderDetails);
        return Json(new {success = "ok ok"});
    }

}