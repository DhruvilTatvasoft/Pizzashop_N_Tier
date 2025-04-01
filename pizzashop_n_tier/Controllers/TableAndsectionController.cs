using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DAL.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


public class TableAndSection : Controller
{
    private readonly ISectionService _sectionService;
    private readonly ITableService _tableService;

    public TableAndSection(ISectionService sectionService, ITableService tableService)
    {
        _sectionService = sectionService;
        _tableService = tableService;
    }
    public IActionResult TableSection()
    {
        return View("TableAndsection");
    }
    public IActionResult SectionData()
    {
        TableAndSectionViewModel model = new TableAndSectionViewModel();
        model.sections = _sectionService.getAllSections();
        return PartialView("_section", model);
    }

    
    public IActionResult LoadTableDataForSection(int sectionId, int pageNumber = 1, int pageSize = 2)
{
  
    if(pageNumber <= 0){
        pageNumber = 1;
    }
    if(pageNumber > _tableService.getAllTables()/pageSize){
        pageNumber = (int)Math.Ceiling((double) _tableService.getAllTables()/pageSize);
    }
    TableAndSectionViewModel model = new TableAndSectionViewModel
    {
        tables = _tableService.getTablesForsection(sectionId, pageNumber, pageSize),
        PageNumber = pageNumber,
        PageSize = pageSize,
        sectionId = sectionId,
        TotalTables = _tableService.getAllTables()
    };

    return PartialView("_tables", model);
}
   
    public IActionResult loadTablePage(int sectionId)
    {
        TableAndSectionViewModel model = new TableAndSectionViewModel();
        model.sectionId = sectionId;
        model.PageNumber = 1;
        model.sections = _sectionService.getAllSections();
        return PartialView("_TableContainer", model);
    }
    public IActionResult AddNewSection(TableAndSectionViewModel model)
    {
        if (!_sectionService.addNewSection(model))
        {
            return Json(new { error = "An error occurred" });
        }
        else if(model.section.Sectionid == 0){

            return Json(new { success = "Section Added Successfully" });
        }
        else
        {
            return Json(new { success = "Section Updated Successfully" });
        }
    }

    public IActionResult updateSectionGet(int sectionId)
    {
        var section = _sectionService.getSection(sectionId);
        if (section == null)
        {
            return NotFound(new { message = "Section not found." });
        }
        return Json(new { section });
    }

    [HttpPost]
    public IActionResult UpdateSection(TableAndSectionViewModel model)
    {
        model.section.Sectionid = model.sectionId;
        
        return PartialView("_section", model);
    }
    [HttpPost]
    public IActionResult deleteSection(int sectionId)
    {
        if (_sectionService.deleteSection(sectionId))
        {
            return RedirectToAction("SectionData");
        }
        else
        {
            return RedirectToAction("SectionData");
        }
    }

    public IActionResult deleteModalGet(int? tableid,List<int>? selectedTables,int? sectionid)
    {
        if(sectionid.HasValue && selectedTables.Count>0){
            foreach(var Tableid in selectedTables){
                if(_tableService.isOccupied(Tableid)){
                    return Json(new {error = "Ocuppied tables can not be deleted"});
                }
            }
        }

        if(tableid.HasValue && _tableService.isOccupied(tableid.Value)){
            return Json(new {error = "Ocuppied table can not be deleted"});
        }
        
        else{
        return PartialView("_deleteModal");
        }
    }

    public IActionResult AddNewTable(TableAndSectionViewModel model)
    {
        if (!_tableService.addNewTable(model.table))
        {
            return Json(new { table = model.table,Error = "Table Already Exist" });
        }
        else
        {
        return Json(new { table = model.table, success= "Table created successfully" });
        }
    }

    [HttpPost]
    public IActionResult deleteTable(List<int>? selectedTables,int? sectionid,int? tableid){
        if(selectedTables != null){
            _tableService.deleteTables(selectedTables);
        }
        if(tableid != null){
            selectedTables.Add(tableid.Value);
            _tableService.deleteTables(selectedTables);
        }
        return Json(new { sectionid });
    }
    [HttpGet]
    public IActionResult updatetableGet(int tableid){
        Table table = _tableService.gettablebyid(tableid);
        Section section = _sectionService.getSectionbyId(table.Sectionid);
        TableAndSectionViewModel model = new TableAndSectionViewModel();
        model.table = table;
        model.section = section;
        model.sections = _sectionService.getAllSections();
        return PartialView("_editTable",model);
    }

    public IActionResult updatetablePost(TableAndSectionViewModel model){
        _tableService.updateTable(model.table);
            return Json(new {model.table.Sectionid});
    }
    
}
