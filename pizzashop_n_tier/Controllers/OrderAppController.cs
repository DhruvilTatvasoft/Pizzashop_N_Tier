

using BAL.Interfaces;
using DAL.Data;
using Microsoft.AspNetCore.Mvc;

namespace pizzashop_n_tier.Views.OrderApp
{
    public class OrderAppController : Controller
    {

        private readonly IMenuService _menuService;

        private readonly IOrderService _orderService;

        public OrderAppController(IMenuService menuService,IOrderService orderService)
        {
            _menuService = menuService;
            _orderService = orderService;
        }

        public IActionResult loadCategoryNavbar(){
            KotViewModel model = new KotViewModel();
            model.categories = _menuService.getAllCategories();
            return PartialView("_category_navbarPartial",model);
        }

        public IActionResult getKot(){
            KotViewModel model = new KotViewModel();
            return PartialView("_kot",model);
        }

        public IActionResult getOrderAppPage(){
            return PartialView("_orderAppHome");
        }

        public IActionResult loadOrdersPerCategory(int categoryid,bool? IsReady){
            KotViewModel model = new KotViewModel();
            model.categories = _menuService.getAllCategories();
            model.orderDetails = _orderService.GetOrderDetailsByCategory(categoryid,IsReady);
            foreach(var orderId in model.orderDetails.Keys){
                model.orderTableSectionDetail = _orderService.getOrderSectionAndTableDetails(orderId);
            }
            Category category = _menuService.getCategoryById(categoryid);
            if(categoryid == 0){
            model.categoryName = "All";
            model.categoryid = 0;
            }
            else{
            model.categoryName = category.Categoryname;
            model.categoryid = category.Categoryid;
            }
            return PartialView("_orderDetailsCard",model);
        }

        public IActionResult loadSingleOrderDetails(int categoryid,int orderid){
            SingleOrderDetailModel model = new SingleOrderDetailModel();
            model = _orderService.getSingleOrderDetail(categoryid,orderid);
            return PartialView("_orderStatusChangeModal",model);
        }

        [HttpPost]
        public IActionResult changeReadyQuantity(SingleOrderDetailModel model){
            _orderService.changeReadyQuantity(model.readyItemCount);
            return Json(new {success="Items are marked as Prepared Successfully"});
         }
    }
}