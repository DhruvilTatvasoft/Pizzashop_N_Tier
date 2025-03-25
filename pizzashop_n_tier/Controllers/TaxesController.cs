using Microsoft.AspNetCore.Mvc;

public class TaxesController : Controller
    {
        private readonly ITaxService _taxService;

        public TaxesController(ITaxService taxService){
            _taxService = taxService;
        }
        public IActionResult showTaxes(){
            return View("taxes");
        }

        public IActionResult Loadtaxes(){
            TaxesViewModel model = new TaxesViewModel();
            model.taxes = _taxService.getAllTaxes();
            return PartialView("_taxesTable",model);
        }
        public IActionResult addEditTaxModalGet(){
            TaxesViewModel model = new TaxesViewModel();
            model.taxes = _taxService.getAllTaxes();
            return PartialView("_addEditTax",model);
        }
[HttpPost]
        public IActionResult AddNewTax(TaxesViewModel model){
            _taxService.addNewTax(model.tax);
            model.taxes = _taxService.getAllTaxes();
            return PartialView("_taxesTable",model);
        }

        [HttpGet]
        public IActionResult deleteTax(string Taxid){
            _taxService.deleteTax(int.Parse(Taxid));
            return Json(new {Taxid});
        }
        
    }
