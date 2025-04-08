

using BAL.Interfaces;
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

        public IActionResult getOrderAppPage(){
            return PartialView("_orderAppHome");
        }
        public IActionResult getKot(){
            KotViewModel model = new KotViewModel();
            model.categories = _menuService.getAllCategories();
            model.orderDetails = _orderService.getAllOrderByOptionFilterForKot();
            foreach(var orderId in model.orderDetails.Keys){
                model.orderTableSectionDetail = _orderService.getOrderSectionAndTableDetails(orderId);
            }
            return PartialView("_kot",model);
            // order.Value.modifiersForItem.Keys.Count
        }
    
    }
}