using BAL.Interfaces;
using DAL.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;

public class MenuController : Controller{

    public readonly IUser _user;

    public readonly ICookieService _cookieService;

    public readonly IEmailGenService _emailService;

    public readonly IMenuService _menuService;


    public readonly IItemService _itemService;

    public readonly IModifierService _modifierService;

    public readonly IImagePath _imageService;

    public MenuController(IImagePath imagePath,IModifierService modifierService, IUser user, IPermissionService permissionService, ICookieService cookieService, IEmailGenService emailService, IMenuService menuService, IItemService itemService)
    {
        _user = user;
        _cookieService = cookieService;
        _emailService = emailService;
        _menuService = menuService;
        _itemService = itemService;

        _modifierService = modifierService;
    }

     public IActionResult Menu()
    {
        return View();
    }

    public IActionResult loadCategoryAndItems()
    {

        return PartialView("_menuPartial3");
    }
    [HttpGet]
    public IActionResult CategoriesData()
    {
        MenuModel model = new MenuModel();
        _menuService.GetCategories(model);
        return PartialView("_menuPartial1", model);
    }

[HttpPost]
    public IActionResult AddCategory(MenuModel model)
    {
        if (ModelState.IsValid)
        {
            var req = HttpContext.Request;
            string email = _cookieService.getValueFromCookie("username", req);
            if (_menuService.addNewcategory(model, email))
            {
                return View("Menu", model);
            }
            else
            {
                TempData["ToastrMessage"] = "Category Already exist";
                TempData["ToastrType"] = "error";
                return View("Menu", model);
            }
        }
        else
        {
            return PartialView("_menuPartial1", model);
        }
    }

    public IActionResult ItemsData(int categoryId)
    {
        Console.WriteLine(categoryId);
        ItemModel model = new ItemModel();
        _itemService.getItemsForcategory(categoryId, model);
        return PartialView("_menuPartial3", model);
    }

    public IActionResult LoadItemPage(int categoryId)
    {
        ItemModel model = new ItemModel();
        model.categoryId = categoryId;
        _itemService.getItemsForcategory(categoryId, model);
        return PartialView("_menuPartial2", model);
    }

    public IActionResult EditCategory(MenuModel model)
    {
        _menuService.GetCategories(model);
        if (ModelState.IsValid)
        {
            var req = HttpContext.Request;
            string email = _cookieService.getValueFromCookie("username", req);
            _menuService.editCategory(model, email);
            TempData["ToastrMessage"] = "Category Updated Successfully";
            TempData["ToastrType"] = "success";

        }
        else if (model.m.categoryName == null || model.m.description == null)
        {
            TempData["ToastrMessage"] = "some fields are neccessary to Fill";
            TempData["ToastrType"] = "Error";
        }
        return View("menu");
    }

    public IActionResult DeleteCategory(int categoryId)
    {
        if (ModelState.IsValid)
        {
            _menuService.deleteCategory(categoryId);
            TempData["ToastrMessage"] = "category deleted Successfully";
            TempData["ToastrType"] = "success";
        }
        else{
        TempData["ToastrMessage"] = "Error occured while deleting category";
        TempData["ToastrType"] = "error";
        }
        return View("Menu");
    }

    [HttpPost]
    public IActionResult AddCategoryPost(MenuModel model, string categoryName, string categoryDesc)
    {
        var req = HttpContext.Request;
        string email = _cookieService.getValueFromCookie("username", req);
        model.m.categoryName = categoryName;
        model.m.description = categoryDesc;
        model.m.categoryId = 56;
        _menuService.addNewcategory(model, email);
        return RedirectToAction("CategoriesData");
    }

    [HttpPost]
    public IActionResult DeleteItems(List<int> selectedItems, int categoryId)
    {
        _itemService.deleteItems(selectedItems);
        Console.WriteLine("items deleted");
        ItemModel model = new ItemModel();
        model.categoryId = categoryId;
        _itemService.getItemsForcategory(categoryId, model);
        return PartialView("_menuPartial3", model);
    }

    [HttpPost]
    public IActionResult SearchItem(string searchedItem, int categoryid)
    {
        ItemModel model = new ItemModel();
        model.items = _itemService.getSearchedItem(searchedItem, model, categoryid);
        model.searchItemName = searchedItem;
        Console.WriteLine("searching works");
        return PartialView("_menuPartial3", model);
    }

    [HttpPost]
    public bool deleteItem(int itemid, int categoryId)
    {
        return _itemService.deleteItem(itemid);
    }

    [HttpGet]
    public IActionResult deleteItem(int itemid)
    {
        return PartialView("_deleteModal");
    }

    [HttpGet]
    public IActionResult OpenAddItemModel()
    {
        ItemModel model = new ItemModel();
        _itemService.getItemsForcategory(1, model);
        return PartialView("_add_edititem", model);
    }

    [HttpPost]
    public IActionResult AddNewItem( ItemModel model)
    {
        model.ModifierModels = JsonConvert.DeserializeObject<List<ModifierModel>>(model.payload);
        
        var req = HttpContext.Request;
        string email = _cookieService.getValueFromCookie("username", req);
        _itemService.addItem(model.i, email);
        _modifierService.addModifiersForItem(model.ModifierModels, model.i.Itemid, email);    
        Console.WriteLine(model.ModifierModels.Count);
            
        foreach(var x in model.ModifierModels){
            Console.WriteLine(x.max_value); 
        }
        return RedirectToAction("ItemsData", new { categoryId = model.i.Categoryid });
    }

    public IActionResult EditItem(int itemid)
    {
        ItemModel model = new ItemModel();
        model.i = _itemService.getItemFromId(itemid);
        model.ModifierModels = _modifierService.getModifiersForItem(itemid);
        return PartialView("_add_edititem", model);
    }

    [HttpGet]
    public IActionResult getModifiers(int modifiergroupId)
    {
        ItemModel model = new ItemModel();
        model.modifiers = _modifierService.getModifiersForMGroup(modifiergroupId);
        model.mg = _modifierService.GetModifiergroup(modifiergroupId);
        return PartialView("_modifiers", model);
    }

    [HttpGet]
    public IActionResult getModifierGroups(string partialViewName)
    {
        ItemModel model = new ItemModel();
        model.modifiergroups = _modifierService.getAllModifierGroups();
        return PartialView(partialViewName, model);
    }

     [HttpGet]
    public IActionResult LoadAllModifiers(){
        ItemModel model = new ItemModel();
        model.modifiers = _modifierService.getAllModifiers();
        return PartialView("_modifierListPartial", model);
    }

    [HttpGet]
    public IActionResult getModifiersForModifierGp(int modifierGroupId)
    {
        ItemModel model = new ItemModel();
        model.modifiers = _modifierService.getModifiersForMGroup(modifierGroupId);
        return PartialView("_modifierListPartial", model);
    }

     public IActionResult LoadModifiersPage(){
        return PartialView("_modifersContainerPartial");
    }
[HttpPost]
    public IActionResult selectedModifiers(List<int> modifierIds){
            List<Modifier> modifiers = new List<Modifier>();
                modifiers = _modifierService.getSelectedModifiers(modifierIds);
            return Json(new {modifiers = modifiers});
    }

[HttpGet]
    public IActionResult SearchModifier(string searchedModifier){
        ItemModel model = new ItemModel();
        model.modifiers = _modifierService.getSearchedModifier(searchedModifier);
        return PartialView("_modifierListPartial",model);
    }

    public IActionResult AddNewModifierGroup(ItemModel model){
       model.ModifierIds = JsonConvert.DeserializeObject<List<int>>(model.payload);
       _modifierService.AddNewModifierGroup(model.mg,model.ModifierIds);
       model.modifiergroups = _modifierService.getAllModifierGroups();
        return PartialView("_modifierGroupsPartial", model);
    }

    public IActionResult DeleteModifier(int modifierid,int modifiergroupid){
        _modifierService.deleteModifier(modifierid,modifiergroupid);
        List<Modifier> modifiers = _modifierService.getModifiersForMGroup(modifiergroupid);
        ItemModel model = new ItemModel();
        model.modifiers = modifiers;
        return View("_modifierListPartial",model);
    }
    public IActionResult EditModifierGroupGet(int modifiergroupid){
        ItemModel model = new ItemModel();
        model.mg = _modifierService.GetModifiergroup(modifiergroupid);
        model.modifiers = _modifierService.getModifiersForMGroup(modifiergroupid);
        return PartialView("_edit_modifierGroup",model);
    }

    public IActionResult updateModifierGroup(ItemModel model){

        model.ModifierIds = JsonConvert.DeserializeObject<List<int>>(model.payload);
        _modifierService.updateModifierGroup(model.mg, model.ModifierIds);
        return View("Menu");
    }

    public IActionResult deleteModifierGroup(int modifierGroupId){
        _modifierService.deleteModifierGroup(modifierGroupId);
        ItemModel model = new ItemModel();
         model.modifiergroups = _modifierService.getAllModifierGroups();
        return View("_modifierGroupsPartial",model);
    }
}