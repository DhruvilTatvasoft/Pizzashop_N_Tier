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
        public IActionResult showOrderDetails(){
            OrderViewModel model = new OrderViewModel();
            model.orders = _orderService.getAllOrders();
            return PartialView("_orderTable",model);
        }

        public IActionResult showOrderDetailsByFilter(int? status=0,string? searchedOrder="",string? filterBy="All Time",DateTime? startDate=null,DateTime? endDate=null){
            OrderViewModel model = new OrderViewModel();
            model.orders = _orderService.getOrdersByFilters(status,searchedOrder,filterBy,startDate,endDate);
            return PartialView("_orderTable",model);
        }
        public IActionResult ExportData(string? searchedOrder="",int? searchbystatus=1,string searchByPeriod="All Time",DateTime? startDate=null,DateTime? endDate=null){
            OrderViewModel model = new OrderViewModel();
            _orderService.createExcelSheet(searchbystatus,searchedOrder,searchByPeriod,startDate,endDate);
            return PartialView("_orderTable",model);
        }

       
    }
}