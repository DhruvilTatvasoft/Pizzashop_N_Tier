using Microsoft.AspNetCore.Routing;
using BAL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using SelectPdf;
using System.Threading.Tasks;



namespace pizzashop_n_tier.Controllers
{

    public class OrderController : Controller
    {
        private readonly ITempDataProvider _tempDataProvider;

        private readonly IOrderService _orderService;

        private IRazorViewEngine _RazorViewEngine;
        private IServiceProvider _serviceProvider;

        private LinkGenerator _linkGenerator;


        public OrderController(IRazorViewEngine RazorViewEngine, LinkGenerator linkGenerator, IOrderService orderService, ITempDataProvider tempDataProvider, IServiceProvider serviceProvider)
        {
            _orderService = orderService;
            _RazorViewEngine = RazorViewEngine;
            _tempDataProvider = tempDataProvider;
            _serviceProvider = serviceProvider;
            _linkGenerator = linkGenerator;
        }

        public IActionResult showOrders()
        {
            OrderViewModel model = new OrderViewModel();
            model.status = _orderService.getAllStatus();
            return View("orders", model);
        }
        

        public IActionResult showOrderDetailsByFilter(int? status = 0, string? searchedOrder = "", string? filterBy = "All Time", DateTime? startDate = null, DateTime? endDate = null,int pageNumber=1,int pageSize=4,string sortBy="orderid",string sortOrder="asc")
        {
            OrderViewModel model = new OrderViewModel();
            // model.orders = _orderService.getOrdersByFilters(status, searchedOrder, filterBy, startDate, endDate,pageNumber,pageSize,sortOrder,sortBy);
            // model.PageSize = pageSize;
            // model.PageNumber = pageNumber;
            // model.TotalOrders = _orderService.getTotalOrderCount();
            // model.sortBy = sortBy;
            // model.sortOrder = sortOrder;
            model = _orderService.getOrdersByFilters(status, searchedOrder, filterBy, startDate, endDate,pageNumber,pageSize,sortOrder,sortBy);
            return PartialView("_orderTable", model);
        }
        public IActionResult ExportData(string? searchedOrder = "", int? searchbystatus = 1, string searchByPeriod = "All Time", DateTime? startDate = null, DateTime? endDate = null)
        {
            OrderViewModel model = new OrderViewModel();
            _orderService.createExcelSheet(searchbystatus, searchedOrder, searchByPeriod, startDate, endDate);
            return Json(new {success = "Exported successfully !!"});
        }

        public IActionResult showOrderDetailsView(int orderid)
        {
            OrderViewModel model = new OrderViewModel();

            model.order = _orderService.getOrderDetails(orderid);
            orderItemModifierViewModel model2 = new orderItemModifierViewModel();
            model2.modifiersForItem = _orderService.getItemsAndModifiers(orderid);
            model.orderedItemModifiers = model2;
            return View("orderDetails", model);
        }

        public async Task<IActionResult> generatePdf(int orderid)
        {
            OrderViewModel model = new OrderViewModel();

            model.order = _orderService.getOrderDetails(orderid);
            orderItemModifierViewModel model2 = new orderItemModifierViewModel();
            model2.modifiersForItem = _orderService.getItemsAndModifiers(orderid);
            model.orderedItemModifiers = model2;
            model.status = _orderService.getAllStatus();

            var viewHtml = await ViewToStringAsync("Order/invoice.cshtml", model);

            try
            {

                HtmlToPdf converter = new HtmlToPdf();

                converter.Options.PdfPageSize = PdfPageSize.A4;
                converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;
                HttpRequest req = HttpContext.Request;
                 string baseUrl = $"{req.Scheme}://{req.Host}";

                PdfDocument doc = converter.ConvertHtmlString(viewHtml,baseUrl);
                using (var memoryStream = new MemoryStream())
                {
                    doc.Save(memoryStream);
                    doc.Close();
                    return File(memoryStream.ToArray(), "application/pdf", $"invoice.pdf");
                }
            }
            catch (Exception ex)
            {
                return View("invoice");
            }
        }
        private async Task<string> ViewToStringAsync<TModel>(string viewName, TModel model)
        {
            try
            {
                ViewDataDictionary viewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                {
                    Model = model
                };
                var httpContext = new DefaultHttpContext
                {
                    RequestServices = _serviceProvider
                };

                var actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor());
                httpContext.SetEndpoint(new Endpoint((_) => Task.CompletedTask, new EndpointMetadataCollection(), "invoice"));
                if (actionContext == null)
                {
                    throw new InvalidOperationException("ActionContext cannot be null");
                }

                var viewResult = _RazorViewEngine.GetView(null, "~/Views/Order/invoice.cshtml", false);
                if (!viewResult.Success)
                {
                    var searchedLocations = string.Join(", ", viewResult.SearchedLocations);
                    throw new InvalidOperationException($"View not found. Searched locations: {searchedLocations}");
                }

                using (var sw = new StringWriter())
                {
                    var viewContext = new ViewContext(actionContext, viewResult.View, viewData, new TempDataDictionary(actionContext.HttpContext, _tempDataProvider), sw, new HtmlHelperOptions());
                    await viewResult.View.RenderAsync(viewContext);
                    return sw.ToString();
                }
            }
            catch (Exception ex)
            {
                return $"Error Message : {ex.Message}";
            }
        }
    }
}