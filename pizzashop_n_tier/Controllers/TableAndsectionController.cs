using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


    public class TableAndSection : Controller
    {
        private readonly ISectionService _sectionService;
        private readonly ITableService _tableService;

        public TableAndSection(ISectionService sectionService,ITableService tableService){
            _sectionService = sectionService;
            _tableService = tableService;
        }
        public IActionResult TableSection(){
            return View("TableAndsection");
        }
        public IActionResult SectionData(){
            TableAndSectionViewModel model=new TableAndSectionViewModel();
            model.sections = _sectionService.getAllSections();
            return PartialView("_section",model);
        }
        public IActionResult LoadTableDataForSection(int sectionId){
            TableAndSectionViewModel model = new TableAndSectionViewModel();
            model.tables = _tableService.getTablesForsection(sectionId);
            model.sectionId = sectionId;
            return PartialView("_tables",model);
        }
        public IActionResult loadTablePage(int sectionId){
            TableAndSectionViewModel model = new TableAndSectionViewModel();
            model.sectionId = sectionId;
            return PartialView("_TableContainer",model);
        }

        public IActionResult AddNewSection(TableAndSectionViewModel model){
            if(_sectionService.addNewSection(model)){

            }
            else{

            }
        }
    }
