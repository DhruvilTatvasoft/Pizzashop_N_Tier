

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

        public IActionResult getKot(){
            KotViewModel model = new KotViewModel();
            return PartialView("_kot",model);
        }

        public IActionResult getOrderAppPage(){
            return PartialView("_orderAppHome");
        }

        public IActionResult getTables(){
            return PartialView("_tableView");
        }

        public IActionResult getWaitingTokenPage(){
            return PartialView("_waitingList");
        }

        public IActionResult getMenuPage(int? tokenid,int? tableid){
            Console.WriteLine("Ok");
            MenuOrderAppModel model = new MenuOrderAppModel();
            model.isTableAssigned = false;
            return PartialView("_menu",model);
        }
    }
}