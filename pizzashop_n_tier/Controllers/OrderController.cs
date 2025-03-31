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

// using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace pizzashop_n_tier.Controllers
{

    public class OrderController : Controller
    {


        // private readonly ICompositeViewEngine _viewEngine;
        private readonly ITempDataProvider _tempDataProvider;
       


        private readonly IOrderService _orderService;

        private IRazorViewEngine _RazorViewEngine;
        private IServiceProvider _serviceProvider;

       
        public OrderController( IRazorViewEngine RazorViewEngine, IOrderService orderService, ITempDataProvider tempDataProvider,IServiceProvider serviceProvider)
        {
            _orderService = orderService;
            // _viewEngine = viewEngine;
            _RazorViewEngine = RazorViewEngine;
            _tempDataProvider = tempDataProvider;
            _serviceProvider = serviceProvider;
        }
        public IActionResult showOrders()
        {
            OrderViewModel model = new OrderViewModel();
            model.status = _orderService.getAllStatus();
            return View("orders", model);
        }
        public IActionResult showOrderDetails()
        {
            OrderViewModel model = new OrderViewModel();
            model.orders = _orderService.getAllOrders();
            return PartialView("_orderTable", model);
        }

        public IActionResult showOrderDetailsByFilter(int? status = 0, string? searchedOrder = "", string? filterBy = "All Time", DateTime? startDate = null, DateTime? endDate = null)
        {
            OrderViewModel model = new OrderViewModel();
            model.orders = _orderService.getOrdersByFilters(status, searchedOrder, filterBy, startDate, endDate);
            return PartialView("_orderTable", model);
        }
        public IActionResult ExportData(string? searchedOrder = "", int? searchbystatus = 1, string searchByPeriod = "All Time", DateTime? startDate = null, DateTime? endDate = null)
        {
            OrderViewModel model = new OrderViewModel();
            _orderService.createExcelSheet(searchbystatus, searchedOrder, searchByPeriod, startDate, endDate);
            return PartialView("_orderTable", model);
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

        public IActionResult generatePdf(int orderid)
        {
            OrderViewModel model = new OrderViewModel();

            model.order = _orderService.getOrderDetails(orderid);
            orderItemModifierViewModel model2 = new orderItemModifierViewModel();
            model2.modifiersForItem = _orderService.getItemsAndModifiers(orderid);
            model.orderedItemModifiers = model2;
            model.orders = _orderService.getAllOrders();
            model.status = _orderService.getAllStatus();

            var ViewHtml = ViewToStringAsync("Order/invoice", model);

            try
            {

                HtmlToPdf converter = new HtmlToPdf();

                converter.Options.PdfPageSize = PdfPageSize.A4;
                converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;

                PdfDocument doc = converter.ConvertHtmlString(ViewHtml.Result);
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
                 var httpContext = new DefaultHttpContext { RequestServices = _serviceProvider };
                var actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor());
                if (actionContext == null)
                {
                    throw new InvalidOperationException("ActionContext cannot be null");
                }
                var viewResult = _RazorViewEngine.FindView(actionContext, viewName, false);
                if (!viewResult.Success)
                {
                    throw new InvalidOperationException($"View {viewName} not found.");
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

        // public IActionResult invoice(int orderid)
        // {
        //     OrderViewModel model = new OrderViewModel();
        //     model.order = _orderService.getOrderDetails(orderid);
        //     orderItemModifierViewModel model2 = new orderItemModifierViewModel();
        //     model2.modifiersForItem = _orderService.getItemsAndModifiers(orderid);
        //     model.orderedItemModifiers = model2;

        //     return View(model);
        // }

        private IView FindView(ActionContext actionContext, string viewName)
        {
            var getViewResult = _RazorViewEngine.GetView(executingFilePath: null, viewPath: viewName, isMainPage: true);
            if (getViewResult.Success)
            {
                return getViewResult.View;
            }

            var findViewResult = _RazorViewEngine.FindView(actionContext, viewName, isMainPage: true);
            if (findViewResult.Success)
            {
                return findViewResult.View;
            }
            var searchedLocations = getViewResult.SearchedLocations.Concat(findViewResult.SearchedLocations);
            var errorMessage = string.Join(
                Environment.NewLine,
                new[] { $"Unable to find view '{viewName}'. The following locations were searched:" }.Concat(searchedLocations)); ;

            throw new InvalidOperationException(errorMessage);
        }
        //   private ActionContext GetActionContext()
        // {
        //     var httpContext = new DefaultHttpContext();
        //     httpContext.RequestServices = _serviceProvider;
        //     return new ActionContext(httpContext, new RouteData(), new ActionDescriptor());
        // }
    }
}