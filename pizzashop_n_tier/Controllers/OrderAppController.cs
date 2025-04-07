

using Microsoft.AspNetCore.Mvc;

namespace pizzashop_n_tier.Views.OrderApp
{
    public class OrderAppController : Controller
    {
        public IActionResult getKot(){
            return View("kot");
        }
    }
}