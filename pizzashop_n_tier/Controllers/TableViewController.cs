using BAL.Interfaces;
using Microsoft.AspNetCore.Mvc;

public class TableViewController : Controller
{
    private readonly ITableService _tableService;
    private readonly ISectionService _sectionService;
    private readonly IWaitingTokenService _waitingTokenService;
    public TableViewController(ITableService tableService, IWaitingTokenService waitingTokenService, ISectionService sectionService)
    {
        _tableService = tableService;
        _waitingTokenService = waitingTokenService;
        _sectionService = sectionService;
    }
    public IActionResult getAllTablesAndSections()
    {
        TableViewModel model = new TableViewModel();
        model = _tableService.GetAllTablesAndSections();
        return PartialView("_tableDetails", model);
    }
    public IActionResult getOffCanvas(int sectionid)
    {
        TableViewModel model = new TableViewModel();
        model.customers = _waitingTokenService.getCustomerTokensForSection(sectionid);
        WaitingTokenModel model2 = new WaitingTokenModel();
        model2.sections = _sectionService.getAllSections();
        model.WaitingToken = model2;
        return PartialView("_assignTableOffcanvasData", model);
    }
}