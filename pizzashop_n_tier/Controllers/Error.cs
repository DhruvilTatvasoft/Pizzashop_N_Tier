using Microsoft.AspNetCore.Mvc;
namespace PizzaShop.Controllers;

public class ErrorController : Controller
{
    
    [HttpGet]
    public IActionResult NotFound()
    {
        return View();
    }
}