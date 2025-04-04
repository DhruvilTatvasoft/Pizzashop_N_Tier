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
    public class CustomerController : Controller
    {
     
        private readonly ICustomerService _customerService;
        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public IActionResult showCustomer(){
            return View("customer");
        }

        public IActionResult loadAllCustomers(int pageSize=4,int pageNumber=1,string sortBy="name",string sortOrder="desc",string? search="",string filterBy = "All Time",DateTime? startDate = null,DateTime? endDate = null){
            CustomerViewModel model = _customerService.getAllCustomers(pageSize,pageNumber,sortBy,sortOrder,search,filterBy,startDate,endDate);
            // model.pageNumber = pageNumber;
            // model.pageSize = pageSize;
            // model.sortBy = sortBy;
            // model.sortOrder = sortOrder;
            
            // model.totalCustomers = _customerService.getAllCustomerCount(search,filterBy);
            return PartialView("_customertable",model);
        }
    }
}