using BAL.Interfaces;
using DAL.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;

public class MenuController : Controller
{

    public readonly IUser _user;

    public readonly ICookieService _cookieService;

    public readonly IEmailGenService _emailService;

    public readonly IMenuService _menuService;


    public readonly IItemService _itemService;

    public readonly IModifierService _modifierService;

    public readonly IImagePath _imageService;

    public MenuController(IImagePath imagePath, IModifierService modifierService, IUser user, IPermissionService permissionService, ICookieService cookieService, IEmailGenService emailService, IMenuService menuService, IItemService itemService)
    {
        _user = user;
        _cookieService = cookieService;
        _emailService = emailService;
        _menuService = menuService;
        _itemService = itemService;

        _modifierService = modifierService;
    }

[Authorize(Policy="CanView_Menu")]
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
    [Authorize(Policy="CanEdit_Menu")]
    public IActionResult AddCategory(MenuModel model)
    {
        var req = HttpContext.Request;
        string email = _cookieService.getValueFromCookie("username", req);
        if (_menuService.addNewcategory(model, email))
        {
            _menuService.GetCategories(model);
            return Json(new { success = "category Added Successfully" });
        }
        else
        {
            return Json(new { error = "Category Already Exist" });
        }


    }

    public IActionResult ItemsData(int categoryId, int pageSize = 4, int pageNumber = 1)
    {
        Console.WriteLine(categoryId);
        ItemModel model = new ItemModel();

        _itemService.getItemsForcategory(categoryId, model, pageSize, pageNumber);
        return PartialView("_menuPartial3", model);
    }

    public IActionResult LoadItemPage(int categoryId)
    {
        ItemModel model = new ItemModel();
        model.categoryId = categoryId;
        // _itemService.getItemsForcategory(categoryId, model);
        return PartialView("_menuPartial2", model);
    }

    [Authorize(Policy="CanEdit_Menu")]
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

    [HttpPost]
    [Authorize(Policy="CanDelete_Menu")]
    public IActionResult DeleteCategory(int categoryId)
    {
        if (ModelState.IsValid)
        {
            _menuService.deleteCategory(categoryId);
            TempData["ToastrMessage"] = "category deleted Successfully";
            TempData["ToastrType"] = "success";
        }
        else
        {
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
        return Json(new { categoryid = categoryId, success = "Items deleted successfully" });
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
    public IActionResult deleteItem(int itemid, int categoryId)
    {
        _itemService.deleteItem(itemid);
        return Json(new { categoryid = categoryId, success = "Item deleted successfully" });
    }

    [HttpGet]
    [Authorize(Policy="CanDelete_Menu")]
    public IActionResult deleteItem(int? itemid)
    {
        return PartialView("_deleteModal");
    }

    [HttpGet]
    [Authorize(Policy="CanEdit_Menu")]
    public IActionResult OpenAddItemModel()
    {

        ItemViewModel model = new ItemViewModel();
        model.categories = _itemService.getAllCategories();
        model.units = _itemService.getAllUnits();
        model.modifiergroups = _itemService.getAllModifierGroups();
        return PartialView("_additem", model);
    }

    [HttpPost]
    [Authorize(Policy="CanEdit_Menu")]
    public IActionResult AddNewItem(ItemViewModel model)
    {
        model.ModifierModels = JsonConvert.DeserializeObject<List<ModifierModel>>(model.payload);
        var req = HttpContext.Request;
        string email = _cookieService.getValueFromCookie("username", req);
        if (!_itemService.addItem(model, email))
        {
            return Json(new { error = "Item already exists", categoryId = model.Categoryid });
        }
        else
        {
            int itemid = _itemService.getItemFromItemName(model.Itemname);
            _modifierService.addModifiersForItem(model.ModifierModels, itemid, email);
            return Json(new { success = "Item Added successfully", categoryid = model.Categoryid });
        }
    }

    // public IActionResult EditItem(int itemid)
    // {
    //     ItemModel model = new ItemModel();
    //     model.i = _itemService.getItemFromId(itemid);
    //     model.ModifierModels = _modifierService.getModifiersForItem(itemid);
    //     return PartialView("_additem", model);
    // }

    [HttpPost]
    public IActionResult PostEditItem(ItemViewModel model)
    {
        model.ModifierModels = JsonConvert.DeserializeObject<List<ModifierModel>>(model.payload);
        _itemService.updateItemdetails(model, model.itemid ?? 1);
        return Json(new { success = "Item Updated successfully", categoryid = model.Categoryid });
    }

    [Authorize(Policy="CanEdit_Menu")]
    public IActionResult EditItemGet(int itemId)
    {
        ItemViewModel model = new ItemViewModel();
        _itemService.loadItemModel(model, itemId);
        return PartialView("_edititem", model);
    }

    // public IActionResult EditItemPost(ItemViewModel model){
    //      model.ModifierModels = JsonConvert.DeserializeObject<List<ModifierModel>>(model.payload);
    //      _itemService.updateItemdetails(model,model.itemid??1);
    //     return View("Menu");
    // }

    [HttpGet]
    public IActionResult getModifiers(int modifiergroupId)
    {
        ItemViewModel model = new ItemViewModel();
        model.modifiers = _modifierService.getModifiersForMGroupForItem(modifiergroupId);
        model.modifiergroup = _modifierService.GetModifiergroup(modifiergroupId);
        return PartialView("_modifiers", model);
    }

    [HttpGet]
    public IActionResult getModifierGroups(string partialViewName)
    {
        ItemModel model = new ItemModel();
        model.modifiergroups = _modifierService.getAllModifierGroups();
        return PartialView(partialViewName, model);
    }

    //  [HttpGet]
    // public IActionResult getModifierGroupsForEdit(string partialViewName)
    // {
    //     ItemViewModel model = new ItemViewModel();
    //     model.modifiergroups = _modifierService.getAllModifierGroups();
    //     return PartialView(partialViewName, model);
    // }


    [HttpGet]
    public IActionResult LoadAllModifiers(int? modifierGroupId,int pageSize = 2,int pageNumber = 1,string search = "")
    {
        ItemModel model = new ItemModel();
        model = _modifierService.getAllModifiers(modifierGroupId,pageSize,pageNumber);
        model.modifierGroupId = modifierGroupId ?? 0;
        return PartialView("_modifierListPartial", model);
    }
    [HttpGet]
    public IActionResult LoadAllModifiersForModifierGroup(int? modifierGroupId,int pageSize = 6,int pageNumber = 1,string search = "")
    {
        ItemModel model = new ItemModel();
        model = _modifierService.getAllModifiers(modifierGroupId,pageSize,pageNumber);
        return PartialView("_modifiersListForModifierGroup", model);
    }

    [HttpGet]
    public IActionResult getModifiersForModifierGp(int modifierGroupId,int pageSize = 2,int pageNumber = 1)
    {
        ItemModel model = new ItemModel();
        model = _modifierService.getModifiersForMGroup(modifierGroupId,pageSize,pageNumber);
        return PartialView("_modifierListPartial", model);
    }

    public IActionResult LoadModifiersPage()
    {
        // ItemModel model = new ItemModel();
        ModifierModel model = new ModifierModel();
        model.modifiergroups = _modifierService.getAllModifierGroups();
        model.units = _modifierService.GetAllUnits();
        return PartialView("_modifersContainerPartial", model);
    }
    [HttpPost]
    public IActionResult selectedModifiers(List<int> modifierIds)
    {
        List<Modifier> modifiers = new List<Modifier>();
        modifiers = _modifierService.getSelectedModifiers(modifierIds);
        return Json(new { modifiers = modifiers });
    }

    [HttpGet]
    public IActionResult SearchModifier(string searchedModifier)
    {
        ItemModel model = new ItemModel();
        model.modifiers = _modifierService.getSearchedModifier(searchedModifier);
        return PartialView("_modifierListPartial", model);
    }
[Authorize(Policy="CanEdit_Menu")]
    public IActionResult AddNewModifierGroup(ModifierModel model)
    {
        model.ModifierIds = JsonConvert.DeserializeObject<List<int>>(model.payload);
        bool isAdded = _modifierService.AddNewModifierGroup(model.mg, model.ModifierIds);
        if (isAdded)
        {
            return Json(new { success = "Modifier Group Added Successfully" });
        }
        else
        {
            return Json(new { error = "Modifier Group Already Exist" });
        }
        
    }
[Authorize(Policy="CanDelete_Menu")]
    public IActionResult DeleteModifier(int modifierid, int modifiergroupid)
    {
        _modifierService.deleteModifier(modifierid, modifiergroupid);
        ItemModel model = new ItemModel();
        model = _modifierService.getModifiersForMGroup(modifiergroupid,1,1);
        return View("_modifierListPartial", model);
    }

[Authorize(Policy="CanEdit_Menu")]
    public IActionResult EditModifierGroupGet(int modifiergroupid)
    {
        ItemModel model = new ItemModel();
        model = _modifierService.getModifiersForMGroup(modifiergroupid,1,1);
        model.mg = _modifierService.GetModifiergroup(modifiergroupid);
        return PartialView("_edit_modifierGroup", model);
    }

    public IActionResult updateModifierGroup(ItemModel model)
    {

        model.ModifierIds = JsonConvert.DeserializeObject<List<int>>(model.payload!)!;
        bool isUpdated = _modifierService.updateModifierGroup(model.mg, model.ModifierIds!);
        if (isUpdated)
        {
            return Json(new { success = "Modifier group Updated successfully " });
        }
        else
        {
            return Json(new { error = "Please change the Modifier Group name because it is already Exist" });
        }
    }
[Authorize(Policy="CanDelete_Menu")]
    public IActionResult deleteModifierGroup(int modifierGroupId)
    {
        _modifierService.deleteModifierGroup(modifierGroupId);
        ItemModel model = new ItemModel();
        model.modifiergroups = _modifierService.getAllModifierGroups();
        return View("_modifierGroupsPartial", model);
    }

    [HttpPost]
    public IActionResult AddNewModifier(ModifierModel model)
    {
        _modifierService.AddNewModifier(model);
        
        return Json(new { success = "Modifier Added successfully ",modifierGroupId = model.Modifiergroupid });
    }

[Authorize(Policy="CanEdit_Menu")]
    public IActionResult EditmodifierGet(int modifierid, int modifierGroupId)
    {
        ModifierModel model = new ModifierModel();
        model.modifiergroups = _modifierService.getAllModifierGroups();
        model.modifier = _modifierService.getModifier(modifierid, modifierGroupId);
        model.Modifiername = model.modifier.Modifiername;
        model.Modifierquantity = model.modifier.Modifierquantity;
        model.Unitid = model.modifier.Unitid;
        model.Modifierrate = model.modifier.Modifierrate;
        model.Modifiergroupid = model.modifier.Modifiergroupid;
        model.Description = model.modifier.Description;
        model.units = _modifierService.GetAllUnits();
        return PartialView("_modifersContainerPartial", model);
    }

    public IActionResult EditmodifierPost(ModifierModel model, int modifierGroupId)
    {
        _modifierService.updateModifier(model, modifierGroupId);
        return View("Menu");
    }
    
    public IActionResult deleteMultipleModifiers(List<int> selectedModifiers,int modifierGroupId){
        foreach (var modifierId in selectedModifiers)
        {
            _modifierService.deleteModifier(modifierId,modifierGroupId);
        }
        return Json(new {modifierGroupId = modifierGroupId});
    }

}