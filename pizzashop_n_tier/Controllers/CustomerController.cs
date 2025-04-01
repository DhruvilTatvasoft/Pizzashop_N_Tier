using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace pizzashop_n_tier.Controllers
{
    public class CustomerController : Controller
    {
     

        public CustomerController()
        {
          
        }

        public IActionResult showCustomers(){
            return View("customer");
        }
        

       
    }
}