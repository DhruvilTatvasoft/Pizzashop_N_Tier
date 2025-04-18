
using DAL.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


public class TableAndSection : Controller
{
    private readonly ISectionService _sectionService;
    private readonly ITableService _tableService;

    public TableAndSection(ISectionService sectionService, ITableService tableService)
    {
        _sectionService = sectionService;
        _tableService = tableService;
    }

    [Authorize(Policy = "CanView_TableAndSection")]
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


    public IActionResult LoadTableDataForSection(int sectionId, int pageNumber = 1, int pageSize = 4, string? searchTable = "")
    {
        TableAndSectionViewModel model = _tableService.getTablesForsection(sectionId, pageNumber, pageSize, searchTable);
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
    [Authorize(Policy = "CanEdit_TableAndSection")]
    public IActionResult AddNewSection(TableAndSectionViewModel model)
    {
        if (!_sectionService.addNewSection(model))
        {
            return Json(new { error = "An error occurred" });
        }
        else if (model.section.Sectionid == 0)
        {

            return Json(new { success = "Section Added Successfully" });
        }
        else
        {
            model.sections = _sectionService.getAllSections();
            return Json(new { success = "Section Updated Successfully",sectionid = model.sections[0].Sectionid });
        }
    }
    [Authorize(Policy = "CanEdit_TableAndSection")]
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
            // return RedirectToAction("SectionData");
            TableAndSectionViewModel model = new TableAndSectionViewModel();
            model.sections = _sectionService.getAllSections();
        if (_sectionService.deleteSection(sectionId))
        {
            return Json(new { success = "Section Deleted Successfully",sectionId =  model.sections[0].Sectionid });
        }
        else
        {
            return Json(new { error = "Section cannot be deleted",sectionId =  model.sections[0].Sectionid });
        }
    }
    [Authorize(Policy = "CanDelete_TableAndSection")]
    public IActionResult deleteModalGet(int? tableid, List<int>? selectedTables, int? sectionid)
    {
        if (sectionid.HasValue && selectedTables.Count > 0)
        {
            foreach (var Tableid in selectedTables)
            {
                if (_tableService.isOccupied(Tableid))
                {
                    return Json(new { error = "Ocuppied tables can not be deleted" });
                }
            }
        }

        if (tableid.HasValue && _tableService.isOccupied(tableid.Value))
        {
            return Json(new { error = "Ocuppied table can not be deleted" });
        }

        else
        {
            return PartialView("_deleteModal");
        }
    }

    [Authorize(Policy = "CanEdit_TableAndSection")]
    public IActionResult AddNewTable(TableAndSectionViewModel model)
    {
        if (!_tableService.addNewTable(model.table))
        {
            return Json(new { table = model.table, Error = "Table Already Exist" });
        }
        else
        {
            return Json(new { table = model.table, success = "Table created successfully" });
        }
    }

    [HttpPost]
    public IActionResult deleteTable(List<int>? selectedTables, int? sectionid, int? tableid)
    {
        if (selectedTables != null)
        {
            _tableService.deleteTables(selectedTables);
        }
        if (tableid != null)
        {
            selectedTables.Add(tableid.Value);
            _tableService.deleteTables(selectedTables);
        }
        return Json(new { sectionid });
    }
    [HttpGet]
    [Authorize(Policy = "CanEdit_TableAndSection")]
    public IActionResult updatetableGet(int tableid)
    {
        Table table = _tableService.gettablebyid(tableid);
        Section section = _sectionService.getSectionbyId(table.Sectionid);
        TableAndSectionViewModel model = new TableAndSectionViewModel();
        model.table = table;
        model.section = section;
        model.sections = _sectionService.getAllSections();
        return PartialView("_editTable", model);
    }

    public IActionResult updatetablePost(TableAndSectionViewModel model)
    {
        _tableService.updateTable(model.table);
        return Json(new { model.table.Sectionid });
    }
}

