using Microsoft.AspNetCore.Mvc;

public class TableViewController : Controller
{
    private readonly ITableService _tableService;
    public TableViewController(ITableService tableService){
        _tableService = tableService;
    }
    public IActionResult getAllTablesAndSections(){
        TableViewModel model = new TableViewModel();
        model = _tableService.GetAllTablesAndSections();
        return PartialView("_tableDetails",model);
    }
}