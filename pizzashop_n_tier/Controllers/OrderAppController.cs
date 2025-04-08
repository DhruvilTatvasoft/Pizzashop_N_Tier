

using Microsoft.AspNetCore.Mvc;

namespace pizzashop_n_tier.Views.OrderApp
{
    public class OrderAppController : Controller
    {

        private readonly IMenuService _menuService;

        public OrderAppController(IMenuService menuService)
        {
            _menuService = menuService;
        }

        public IActionResult getOrderAppPage(){
            return PartialView("_orderAppHome");
        }
        public IActionResult getKot(){
            KotViewModel model = new KotViewModel();
            model.categories = _menuService.getAllCategories();
            return PartialView("_kot",model);
        }
    
    }
}