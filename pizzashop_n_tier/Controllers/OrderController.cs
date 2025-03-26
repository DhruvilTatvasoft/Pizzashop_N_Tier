using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BAL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace pizzashop_n_tier.Controllers
{
    
    public class OrderController : Controller
    {

        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService){
            _orderService = orderService;
        }
        public IActionResult showOrders(){
             OrderViewModel model = new OrderViewModel();
              model.status = _orderService.getAllStatus();
            return View("orders",model);
        }
        public IActionResult showOrderDetails(int? status,string? searchedOrder,string? filterBy){
            OrderViewModel model = new OrderViewModel();
            if(status != null && status != 0){
                model.orders = _orderService.getAllOrdersByStatus(status.Value);
            }
            else if(searchedOrder != null && searchedOrder != ""){
                model.orders = _orderService.getAllOrdersBySearch(searchedOrder.ToString());
            }
            else if(filterBy != null && filterBy != ""){
                model.orders = _orderService.getAllOrderByOptionFilter(filterBy);
            }
            else{
            model.orders = _orderService.getAllOrders();
            }
            return PartialView("_orderTable",model);
        }

       
    }
}