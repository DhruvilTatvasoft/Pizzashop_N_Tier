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

        public TableAndSection(ISectionService sectionService){
            _sectionService = sectionService;
        }
        public IActionResult TableSection(){
            TableAndSectionViewModel model = new TableAndSectionViewModel();
            model.sections = _sectionService.getAllSections() ;

            return View("TableAndsection",model);
        }
    }
