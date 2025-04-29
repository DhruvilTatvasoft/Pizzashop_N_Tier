using BAL.Interfaces;
using DAL.Data;
using Microsoft.AspNetCore.Mvc;

public class KotController : Controller
{
    private readonly IMenuService _menuService;

    private readonly IOrderService _orderService;

    public KotController(IMenuService menuService, IOrderService orderService)
    {
        _menuService = menuService;
        _orderService = orderService;
    }

    public IActionResult loadCategoryNavbar()
    {
        KotViewModel model = new KotViewModel();
        model.categories = _menuService.getAllCategories();
        return PartialView("_category_navbarPartial", model);
    }

    public IActionResult getKot()
    {
        KotViewModel model = new KotViewModel();
        return PartialView("_kot", model);
    }

    public IActionResult loadOrderDetailCardContainer(int categoryid){
        KotViewModel model = new KotViewModel();
        model.categoryid = categoryid;
        return PartialView("_orderDetailsCard",model);
    }

    public IActionResult loadOrdersPerCategory(int categoryid, bool? IsReady)
    {
        KotViewModel model = new KotViewModel();
        model.categories = _menuService.getAllCategories();
        model.orderDetails = _orderService.GetOrderDetailsByCategory(categoryid, IsReady);
        Dictionary<int, tableAndsection> orderTableSectionDetail = new Dictionary<int, tableAndsection>();
        foreach (var order in model.orderDetails.Keys)
        {
            tableAndsection tableAndsection = _orderService.getOrderSectionAndTableDetails(order.Orderid);
           orderTableSectionDetail.Add(order.Orderid,tableAndsection);
        }
        model.orderTableSectionDetail = orderTableSectionDetail;
        Category category = _menuService.getCategoryById(categoryid);
        if (categoryid == 0)
        {
            model.categoryName = "All";
            model.categoryid = 0;
        }
        else
        {
            model.categoryName = category.Categoryname;
            model.categoryid = category.Categoryid;
        }
        return PartialView("_orderCardPartial", model);
    }

    public IActionResult loadSingleOrderDetails(int categoryid, int orderid)
    {
        SingleOrderDetailModel model = new SingleOrderDetailModel();
        // model = _orderService.getSingleOrderDetail(categoryid, orderid,status);
        model.orderid = orderid;
        model.categoryid = categoryid;
        return PartialView("_orderStatusChangeModal", model);
    }

    public IActionResult loadItemsInModalAccordingToStatus(int categoryid, int orderid,string status = "Ready"){
        SingleOrderDetailModel model = new SingleOrderDetailModel();
        model = _orderService.getSingleOrderDetail(categoryid, orderid,status);
        model.categoryid = categoryid;
        model.orderid = orderid;
        return PartialView("_orderItemTable", model);
    }

    [HttpPost]
    public IActionResult changeReadyQuantity(SingleOrderDetailModel model)
    {
        _orderService.changeReadyQuantity(model.readyItemCount);
        return Json(new { success = "Items are marked as Prepared Successfully" });
    }
}