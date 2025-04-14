using BAL.Interfaces;
using Microsoft.AspNetCore.Mvc;

public class WaitingListController : Controller
{
    public ISectionService _sectionService;
    public IWaitingTokenService _waitingTokenService;
    public WaitingListController(ISectionService sectionService,IWaitingTokenService waitingTokenService){
        _sectionService = sectionService;
        _waitingTokenService = waitingTokenService;
    }
    public IActionResult getSectionNavbar(){
        WaitingTokenModel model = new WaitingTokenModel();
        model.Sections = _waitingTokenService.getSectionsWithWaitingTokens();
        model.sections = _sectionService.getAllSections();
        model.TotalWaitingTokens = _waitingTokenService.getTotalWaitingTokens();
        return PartialView("_sectionNavbar", model);

    }

    public IActionResult getwaitingTokens(int sectionId = 0){
        WaitingTokenModel model = _waitingTokenService.getAllWaitingTokens(sectionId);
        return PartialView("_waitingTokentable", model);
    }

    public IActionResult getAddEditTokenModel(){
        WaitingTokenModel model = new WaitingTokenModel();
        model.sections = _sectionService.getAllSections();
        return PartialView("_addEditTokenModel", model);
    }
    public IActionResult AddNewWaitingToken(WaitingTokenModel model){
        if(_waitingTokenService.AddNewWaitingToken(model)){
            return Json(new { success = "Token Created Successfully",sectionid = model.sectionId });
        }
        else{

            return Json(new { error = "Some Error Occured",sectionid = model.sectionId });
        }
        
    }

    public IActionResult GetEditTokenModel(int WaitingTokenId){
        WaitingTokenModel model = _waitingTokenService.getTokenDetails(WaitingTokenId);
        model.sections = _sectionService.getAllSections();
        return PartialView("_addEditTokenModel", model);
    }
    [HttpPost]
    public IActionResult UpdateWaitingToken(WaitingTokenModel model){
        if(_waitingTokenService.UpdateWaitingToken(model)){
            return Json(new {success = "Waiting Token Updated Successfully",sectionid = model.sectionId });
        }
        else{
            return Json(new {error = "Some Error occured",sectionid = model.sectionId });
        }
    }
}