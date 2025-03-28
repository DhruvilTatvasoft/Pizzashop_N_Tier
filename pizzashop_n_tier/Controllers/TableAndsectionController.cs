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
    public IActionResult LoadTableDataForSection(int sectionId)
    {
        TableAndSectionViewModel model = new TableAndSectionViewModel();
        model.tables = _tableService.getTablesForsection(sectionId);
        model.sectionId = sectionId;
        return PartialView("_tables", model);
    }
   
    public IActionResult loadTablePage(int sectionId)
    {
        TableAndSectionViewModel model = new TableAndSectionViewModel();
        model.sectionId = sectionId;
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

    public IActionResult deleteModalGet()
    {
        return PartialView("_deleteModal");
    }

    public IActionResult AddNewTable(TableAndSectionViewModel model)
    {
        if (_tableService.addNewTable(model.table))
        {
            TempData["ToastrMessage"] = "New Table added succesfully";
            TempData["ToastrType"] = "success";
        }
        else
        {
            TempData["ToastrMessage"] = "Error occured";
            TempData["ToastrType"] = "error";
        }
        return Json(new { model.table });
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
