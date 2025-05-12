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


    public MenuOrderAppController(IMenuOrderAppService menuOrderAppService, ITaxService taxesService, ITableService tableService, IItemService itemService, IModifierService modifierService, IMenuService menuService, IOrderService orderService, IWaitingTokenService waitingTokenService)
    {
        _menuOrderAppService = menuOrderAppService;
        _menuService = menuService;
        _waitingTokenService = waitingTokenService;
        _orderService = orderService;
        _itemService = itemService;
        _modifierService = modifierService;
        _tableService = tableService;
        _taxesService = taxesService;
    }
    public IActionResult getMenuSidebar()
    {
        MenuOrderAppModel model = new MenuOrderAppModel();
        model.categories = _menuService.getAllCategories();
        return PartialView("_menuSidebar", model);
    }

    [HttpPost]

    public IActionResult getItemsForCategory([FromBody] orderDetailsForAssignedTable model)
    {
        MenuOrderAppModel responseModel = new MenuOrderAppModel();

        if (model.customerid != 0)
        {
            responseModel.customer = _menuOrderAppService.getcustomerDetails(model.customerid ?? 0, model.TableIds);
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

        responseModel.items = _menuOrderAppService.getItemsForcategory(model.CategoryId, model.itemType, model.SearchedItem);
        responseModel.categoryId = model.CategoryId;

        return PartialView("_itemData", responseModel);
    }


    [HttpPost]
    public IActionResult getOrderDetails([FromBody] assignTableDetails model)
    {

        MenuOrderAppModel responseModel = new MenuOrderAppModel();
        responseModel.customer = _menuOrderAppService.getcustomerDetails(model.customerid ?? 0, model.tableids);
        responseModel.customer.PersonCount = model.totalPersonCount ?? 0;
        responseModel.tables = new List<DAL.Data.Table>();
        List<int> tableids = new List<int>();
        foreach (int tableid in model.tableids)
        {
            DAL.Data.Table table = _tableService.gettablebyid(tableid);
            tableids.Add(tableid);

            responseModel.tables.Add(table);
        }
        responseModel.customer.tableids = tableids;
        responseModel.taxesandfees = _taxesService.getAllTaxes();
        responseModel.tokenid = model.tokenid ?? 0;
        responseModel.orderid = model.orderid ?? 0;
        if (model.orderid != 0 && model.orderid != null)
        {
            _menuOrderAppService.loadOrderedItemsData(model.orderid, responseModel);
        }

        return PartialView("_orderDetailModal", responseModel);
    }
    public IActionResult getMenuDataContainer()
    {
        MenuOrderAppModel model = new MenuOrderAppModel();
        model.categories = _menuService.getAllCategories();
        return PartialView("_menuItems", model);
    }
    public IActionResult getItemDetails(int itemid, string isTableAssigned = "False")
    {
        MenuOrderAppModel model = new MenuOrderAppModel();
        model.modifiersForItem = _menuOrderAppService.getModifiersForItem(itemid);
        model.item = _menuOrderAppService.getItem(itemid);
        if (isTableAssigned == "True")
        {
            model.isTableAssigned = true;
        }
        else
        {
            model.isTableAssigned = false;
        }
        return PartialView("_itemDetailsModal", model);
    }

    public IActionResult showCustomerDetails(int customerid, int totalPersonCount)
    {
        MenuOrderAppModel model = new MenuOrderAppModel();
        List<int> tableid = new List<int> { 0 };
        model.customer = _menuOrderAppService.getcustomerDetails(customerid, null);
        model.customer.PersonCount = totalPersonCount;
        return PartialView("_customerDetailModal", model);
    }
    [HttpPost]
    public IActionResult editCustomerDetails(MenuOrderAppModel model)
    {
        _menuOrderAppService.saveCustomerDetails(model.customer);
        return Json(new { success = "Customer Details Save successfully" });
    }

    [HttpGet]
    public IActionResult GenerateQRCode(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return BadRequest("Text is required to generate a QR code.");
        }
        var qrCodeImage = _menuOrderAppService.GenerateQRCode(text);
        return File(qrCodeImage, "image/png");
    }
    [HttpPost]
    public IActionResult addOrRemoveFromFavorites(int itemid,string process)
    {
        _itemService.addOrRemoveFromFavorites(itemid,process);
        return Ok();
    }

    [HttpPost]
    public IActionResult addItemInOrder(int itemid, List<int> modifiers, string uniqueId, int? orderid)
    {
        MenuOrderAppModel model = new MenuOrderAppModel();
        model.item = _itemService.getItemFromId(itemid);
        if (orderid != 0)
        {
            _menuOrderAppService.getOrderdItemQuantity(orderid, itemid, model, modifiers);
        }
        List<Modifier> modifierList = new List<Modifier>();
        foreach (int modifierid in modifiers)
        {
            Modifier modifier = _modifierService.getModifierFromId(modifierid);
            modifierList.Add(modifier);
        }
        model.modifiers = modifierList;
        model.uniqueId = uniqueId;
        model.orderid = orderid ?? 0;


        return PartialView("_itemAccordian", model);
    }

    [HttpPost]
    public IActionResult saveTheOrderDetails([FromBody] OrderDetailsViewModel orderDetails)
    {
        int orderid = _menuOrderAppService.createOrder(orderDetails);
        return Json(new { success = "Order Saved Successfully", Orderid = orderid });
    }

    public IActionResult getRunningTableOrder(int tableid)
    {
        MenuOrderAppModel model = _menuOrderAppService.getRunningTableOrder(tableid);
        return PartialView("_menu", model);
    }

    public IActionResult getOrderDetailsForAssignedTable(int tableid){
        MenuOrderAppModel model = _menuOrderAppService.getAssignedTableDetails(tableid);
        return PartialView("_menu",model);
    }

    [HttpPost]
    public IActionResult completeTheOrder([FromBody] ItemDetail Itemdetails)
    {
        if (_menuOrderAppService.completeTheOrder(Itemdetails))
        {
            return Json(new { success = "Order completed" });
        }
        else
        {
            return Json(new { error = "Some items are not ready yet" });
        }
    }

    [HttpPost]
    public IActionResult cancelTheOrder([FromBody] ItemDetail Itemdetails)
    {
        if (_menuOrderAppService.cancelTheOrder(Itemdetails))
        {
            return Json(new { success = "Order cancelled successfully" });
        }
        else
        {

            return Json(new { success = "Order can not cancelled as some items of the order are ready" });
        }
    }

}