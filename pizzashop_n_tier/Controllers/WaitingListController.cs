using BAL.Interfaces;
using Microsoft.AspNetCore.Mvc;

public class WaitingListController : Controller
{
    public ISectionService _sectionService;
    public IWaitingTokenService _waitingTokenService;
    public IOrderService _orderService;
    public WaitingListController(ISectionService sectionService, IWaitingTokenService waitingTokenService,IOrderService orderService)
    {
        _sectionService = sectionService;
        _waitingTokenService = waitingTokenService;
        _orderService = orderService;
    }
    public IActionResult getSectionNavbar()
    {
        WaitingTokenModel model = new WaitingTokenModel();
        model.Sections = _waitingTokenService.getSectionsWithWaitingTokens();
        model.sections = _sectionService.getAllSections();
        model.TotalWaitingTokens = _waitingTokenService.getTotalWaitingTokens();
        return PartialView("_sectionNavbar", model);

    }
    public IActionResult getwaitingTokens(int sectionId = 0)
    {
        WaitingTokenModel model = _waitingTokenService.getAllWaitingTokens(sectionId);

        return PartialView("_waitingTokentable", model);
    }

    public IActionResult getAddEditTokenModel()
    {
        WaitingTokenModel model = new WaitingTokenModel();
        model.sections = _sectionService.getAllSections();
        return PartialView("_addEditTokenModel", model);
    }
    [HttpPost]
    public IActionResult AddNewWaitingToken(WaitingTokenModel model)
    {
        if (_waitingTokenService.AddNewWaitingToken(model))
        {
            return Json(new { success = "Token Created Successfully", sectionid = model.sectionId });
        }
        else
        {

            return Json(new { error = "Token with this Email Id is Already Created.Use other Email Address", sectionid = model.sectionId });
        }

    }
    public IActionResult GetEditTokenModel(int WaitingTokenId)
    {
        WaitingTokenModel model = _waitingTokenService.getTokenDetails(WaitingTokenId);
        model.sections = _sectionService.getAllSections();
        return PartialView("_addEditTokenModel", model);
    }
    [HttpPost]
    public IActionResult UpdateWaitingToken(WaitingTokenModel model)
    {
        if (_waitingTokenService.UpdateWaitingToken(model))
        {
            return Json(new { success = "Waiting Token Updated Successfully", sectionid = model.sectionId });
        }
        else
        {
            return Json(new { error = "Some Error occured", sectionid = model.sectionId });
        }
    }
    public IActionResult getSuggestions(string name)
    {
        WaitingTokenModel model = new WaitingTokenModel();
        model.customerList = _waitingTokenService.getSuggestedCustomerList(name);
        return PartialView("_SuggessionPartial", model);
    }

    [HttpPost]
    public IActionResult deleteWaitingToken(int tokenid)
    {
        int Sectionid = _waitingTokenService.getSectionIdOfToken(tokenid);
        if (_waitingTokenService.deleteWaitingToken(tokenid))
        {
            return Json(new { success = "Waiting Token Deleted Successfully", sectionid = Sectionid });
        }
        return Json(new { error = "Some error Occurred", sectionid = Sectionid });
    }
    public IActionResult getAssignTableModel(int tokenid)
    {
        WaitingTokenModel model = new WaitingTokenModel();
        model.sections = _sectionService.getAllSections();
        model.sectionId = _waitingTokenService.getSectionIdOfToken(tokenid);
        model.tables = _waitingTokenService.getTablesForToken(tokenid);
        model.tokenId = tokenid;
        return PartialView("_assignTableModel", model);
    }

    public IActionResult AssignTable(int tableid, int tokenid)
    {
        if (_waitingTokenService.AssignTable(new List<int> { tableid }, tokenid))
        {
            _orderService.CreateOrder(tokenid, tableid);
            return Json(new { success = "Table Assigned Successfully" });
        }
        else
        {
            return Json(new { error = "Some Error occured " });
        }
    }

   
}