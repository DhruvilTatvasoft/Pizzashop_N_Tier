using BAL.Interfaces;
using DAL.Data;
using Microsoft.AspNetCore.Mvc;

public class TableViewController : Controller
{
    private readonly ITableService _tableService;
    private readonly ISectionService _sectionService;
    private readonly IWaitingTokenService _waitingTokenService;
    private readonly ICustomerService _customerService;
    private readonly IOrderService _orderService;
    public TableViewController(ITableService tableService, ICustomerService customerService, IWaitingTokenService waitingTokenService, ISectionService sectionService, IOrderService orderService)
    {
        _tableService = tableService;
        _waitingTokenService = waitingTokenService;
        _sectionService = sectionService;
        _orderService = orderService;
        _customerService = customerService;
    }
    public IActionResult getAllTablesAndSections()
    {
        TableViewModel model = new TableViewModel();
        model = _tableService.GetAllTablesAndSections();
        return PartialView("_tableDetails", model);
    }
    [HttpPost]
    public IActionResult getOffCanvas(int sectionid, List<int> tableids)
    {
        TableViewModel model = new TableViewModel();
        model.customers = _waitingTokenService.getCustomerTokensForSection(sectionid, tableids);
        WaitingTokenModel model2 = new WaitingTokenModel();
        model2.sections = _sectionService.getAllSections();
        model.WaitingToken = model2;
        model.tables = tableids;
        model.maxPersonCount = _waitingTokenService.getMaxPersonCountForSection(tableids);
        model.sectionid = sectionid;
        return PartialView("_assignTableOffcanvasData", model);
    }

    [HttpPost]
    public IActionResult assignTable([FromBody] assignTableDetails Model)
    {
        MenuOrderAppModel model = new MenuOrderAppModel();
        if (Model.customerModal != null)
        {
            Customer createdCustomer = _waitingTokenService.createNewCustomer(Model.customerModal);
            model.customer = Model.customerModal;
            model.customer.customerId = createdCustomer.Customerid;
        }
        else
        {
            model.tokenid = Model.tokenid ?? 0;
            model.customer = _waitingTokenService.getCustomerForWaitingToken(Model.tokenid ?? 0, Model.tableids);
            _waitingTokenService.AssignTable(Model.tableids, Model.tokenid ?? 0);
        }
        model.isTableAssigned = true;
        model.categoryId = 0;
        return PartialView("_menu", model);
    }
}