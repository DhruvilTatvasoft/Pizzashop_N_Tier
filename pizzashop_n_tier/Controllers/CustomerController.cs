using BAL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace pizzashop_n_tier.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;
        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public IActionResult showCustomer()
        {
            return View("customer");
        }

        public IActionResult loadAllCustomers(int pageSize = 4, int pageNumber = 1, string sortBy = "name", string sortOrder = "asc", string? search = "", string filterBy = "All Time", DateTime? startDate = null, DateTime? endDate = null)
        {
            CustomerViewModel model = _customerService.getAllCustomers(pageSize, pageNumber, sortBy, sortOrder, search, filterBy, startDate, endDate);
            return PartialView("_customertable", model);
        }

        public IActionResult getCustomerHistory(int customerid)
        {
            CustomerViewModel model = _customerService.getCustomerHistory(customerid);
            return PartialView("_customerHistory", model);
        }

        public IActionResult ExportCustomerDetails(int pageSize = 4, int pageNumber = 1, string sortBy = "name", string sortOrder = "desc", string? search = "", string filterBy = "All Time", DateTime? startDate = null, DateTime? endDate = null, bool isExport = true)
        {
            var fileBytes = _customerService.exportCustomerDetails(pageSize, pageNumber, sortBy, sortOrder, search ?? string.Empty, filterBy, startDate, endDate, true);
            if (fileBytes == null || fileBytes.Length == 0)
                return BadRequest("Failed to generate Excel file");

            string fileName = $"CustomerDetails_{DateTime.Now:yyyy-MM-dd-HH-mm-ss}.xls";
            return File(fileBytes, "application/vnd.ms-excel", fileName);
        }

    }
}
