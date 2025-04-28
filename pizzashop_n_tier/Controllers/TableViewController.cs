using BAL.Interfaces;
using Microsoft.AspNetCore.Mvc;

public class TableViewController : Controller
{
    private readonly ITableService _tableService;
    private readonly ISectionService _sectionService;
    private readonly IWaitingTokenService _waitingTokenService;
    private readonly IOrderService _orderService;
    public TableViewController(ITableService tableService, IWaitingTokenService waitingTokenService, ISectionService sectionService,IOrderService orderService)
    {
        _tableService = tableService;
        _waitingTokenService = waitingTokenService;
        _sectionService = sectionService;
        _orderService = orderService;
    }
    public IActionResult getAllTablesAndSections()
    {
        TableViewModel model = new TableViewModel();
        model = _tableService.GetAllTablesAndSections();
        return PartialView("_tableDetails", model);
    }
    [HttpPost]
    public IActionResult getOffCanvas(int sectionid,List<int> tableids)
    {
        TableViewModel model = new TableViewModel();
        model.customers = _waitingTokenService.getCustomerTokensForSection(sectionid,tableids);
        WaitingTokenModel model2 = new WaitingTokenModel();
        model2.sections = _sectionService.getAllSections();
        model.WaitingToken = model2;
        model.tables = tableids;
        return PartialView("_assignTableOffcanvasData", model);
    }

    [HttpPost]
    public IActionResult assignTable([FromBody]assignTableDetails Model)
    {
        MenuOrderAppModel model = new MenuOrderAppModel();
        model.customer = _waitingTokenService.getCustomerForWaitingToken(Model.tokenid,Model.tableids);
        model.isTableAssigned = true;
        model.tokenid = Model.tokenid;
        model.categoryId = 0;
        return PartialView("_menu",model);
    }   
}