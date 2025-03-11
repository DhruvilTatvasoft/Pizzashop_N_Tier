using System.Reflection.Metadata.Ecma335;
using AspNetCoreGeneratedDocument;
using DAL.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

public class DashboardController : Controller
{

    public readonly ILogin _log;

    public readonly IUser _user;

    public readonly ICookieService _cookieService;

    public readonly IEmailGenService _emailService;

    public readonly IMenuService _menuService;

    private readonly ICookieService _CookieService;

    public readonly IItemService _itemService;
    public DashboardController(ILogin log, IUser user, ICookieService cookieService, IEmailGenService emailService, IMenuService menuService, IItemService itemService)
    {
        _log = log;
        _user = user;
        _cookieService = cookieService;
        _emailService = emailService;
        _menuService = menuService;
        _itemService = itemService;
    }
    public IActionResult ShowDashboard()
    {
        return View();
    }
    public IActionResult Myprofile()
    {

        var req = HttpContext.Request;
        string email = _cookieService.getValueFromCookie("username", req);
        // Console.WriteLine("email from session" + email);
        var user = _log.getUser(email);
        UserDetailModel m = _log.setUserInModel(user);
        return View(m);
    }
    [HttpPost]
    public IActionResult updateProfile(UserDetailModel model, IFormFile profilePicPath)
    {
        model.Role = _user.getRoles();
        model.Country = _user.getAllCountries();
        ModelState.Remove("city");
        ModelState.Remove("role");
        ModelState.Remove("email");
        ModelState.Remove("state");
        ModelState.Remove("status");
        ModelState.Remove("password");
        ModelState.Remove("profilePicPath");
        ModelState.Remove("ProfilePath");
        if (ModelState.IsValid)
        {
            var req = HttpContext.Request;
            string email = _cookieService.getValueFromCookie("username", req);
            Console.WriteLine("in update profile");
            _user.updateUser(model, email);
            TempData["ToastrMessage"] = "profile updated successfully";
            TempData["ToastrType"] = "success";
            return RedirectToAction("Myprofile");
        }
        else
        {
            TempData["ToastrMessage"] = "Some fields are neccessary to Fill";
            TempData["ToastrType"] = "error";
            return View("Myprofile", model);
        }
    }
    [HttpGet]
    public IActionResult ResetPassword()
    {
        return View();
    }

    [HttpPost]
    public IActionResult ResetPassword(chang_p_model model)
    {
        var req = HttpContext.Request;
        string email = _cookieService.getValueFromCookie("username", req);
        string password = _cookieService.getValueFromCookie("password", req);
        if (_user.changePass(req, model, email, password))
        {
            var res = HttpContext.Response;
            _CookieService.setInCookie(model.newpass, res, "password");
            TempData["ToastrMessage"] = "Password Changed successfully";
            TempData["ToastrType"] = "success";
            return RedirectToAction("index", "LoginController");
        }
        else
        {
            ModelState.AddModelError("oldpass", "Please enter correct Password");
            TempData["ToastrMessage"] = "Incorrect current password";
            TempData["ToastrType"] = "error";
            return View(model);
        }
    }
    public IActionResult Logout()
    {
        HttpContext.Response.Cookies.Delete("token");
        HttpContext.Response.Cookies.Delete("username");
        TempData["ToastrMessage"] = "Logout successfully";
        TempData["ToastrType"] = "success";
        return RedirectToAction("Index", "Login");
    }

    [HttpGet]
    public IActionResult AddUser()
    {
        UserDetailModel model = new UserDetailModel();
        model.Role = _user.getRoles();
        model.Country = _user.getAllCountries();
        return View(model);
    }
    [HttpPost]
    public IActionResult AddUser(UserDetailModel model)
    {
        model.Role = _user.getRoles();
        model.Country = _user.getAllCountries();
        if (_user.IsUserExist(model.email))
        {
            TempData["ToastrMessage"] = "User already exist";
            TempData["ToastrType"] = "error";
            return View(model);
        }
        ModelState.Remove("city");
        ModelState.Remove("Status");
        ModelState.Remove("state");
        ModelState.Remove("profilePicPath");
        ModelState.Remove("ProfilePath");
        if (ModelState.IsValid)
        {
            var req = HttpContext.Request;
            string email = _cookieService.getValueFromCookie("username", req);
            Console.WriteLine("in post method");
            _user.saveNewUser(model, email);
            _emailService.emailForForgetPass(req, model.email, model.password);
            TempData["ToastrMessage"] = "New User added Successfully";
            TempData["ToastrType"] = "success";
            return View("showUsers");
        }
        else
        {
            Console.WriteLine(ModelState.ErrorCount);
            model.Role = _user.getRoles();
            model.Country = _user.getAllCountries();
            TempData["ToastrMessage"] = "some fields are neccessary to Fill";
            TempData["ToastrType"] = "error";
            return View(model);
        }
    }

    public IActionResult getUsers(string search, int maxRows = 5, int currentPage = 1)
    {
        userpagingdetailmodel model = new userpagingdetailmodel();
        model = _user.loadusers(model, currentPage, maxRows, search);
        return PartialView("_TablePartialView", model);
    }
    public IActionResult getSearchedUser(string search, int maxRows = 5, int currentPage = 1)
    {
        userpagingdetailmodel model = new userpagingdetailmodel();
        Console.WriteLine("searching user");
        model = _user.loadusers(model, currentPage, maxRows, search);
        Console.WriteLine("searched");
        return PartialView("_TablePartialView", model);
    }

    [HttpGet]
    public IActionResult GetStates(int countryId)
    {
        var states = _user.getStates(countryId);
        return Json(new SelectList(states, "Stateid", "Statename"));
    }
    [HttpGet]
    public IActionResult DeleteUser(int Id)
    {
        _user.deleteUser(Id);
        TempData["ToastrMessage"] = "User Deleted successfully";
        TempData["ToastrType"] = "success";
        return View("showUsers");
    }
    [HttpGet]
    public IActionResult EditUser(int Id)
    {

        UserDetailModel model = new UserDetailModel();
        Console.WriteLine("In edit user");
        model = _user.getUserDetails(Id);
        return View(model);
    }
    [HttpPost]
    public IActionResult EditUser(UserDetailModel model, int Id)
    {
        // ModelState.Remove("city");
        // ModelState.Remove("ProfilePath");
        // ModelState.Remove("password");
        // ModelState.Remove("email");
        // ModelState.Remove("Status");
        if (ModelState.IsValid)
        {
            _user.updateUser(model, Id);
            TempData["ToastrMessage"] = "User updated successfully";
            TempData["ToastrType"] = "success";
            return View("showUsers");
        }
        else
        {
            model = _user.getUserDetails(Id);
            TempData["ToastrMessage"] = "some fields are required";
            TempData["ToastrType"] = "error";
            return View(model);
        }
    }

    // JSON endpoint: Get Cities for a State
    [HttpGet]
    public IActionResult GetCities(int stateId)
    {
        var cities = _user.getStateCities(stateId);
        return Json(new SelectList(cities, "Cityid", "Cityname"));
    }
    [HttpGet]
    public IActionResult Roles()
    {
        RolesModel model = new RolesModel();
        model.Role = _user.getAllRoles();
        return View(model);
    }
    public IActionResult showUsers()
    {
        return View();
    }

    [HttpGet]
    public IActionResult PermissionsOfRole(int Id)
    {
        //     PermissionsModel model = new PermissionsModel();
        //     model = _user.permissionsForRole(Id);
        //     return View("permissions",model);
        PermissionsModel2 model = new PermissionsModel2();
        model.roleid = Id;
        model = _user.permissionsForRole(Id);
        return View("permissions", model);
    }

    // [HttpGet]
    //     public IActionResult PermissionOfRole(int Id){

    //     }
    [HttpPost]
    public IActionResult UpdatePermissions(PermissionsModel model)
    {
        _user.updatePermissions(model);
        return RedirectToAction("permissionsOfRole", "Dashboard");
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
        // model.searchItemName = "dfdf";
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

        return PartialView("_menuPartialView1", model);


        // return PartialView("_menuPartial1", model);

    }
    public IActionResult DeleteCategory(int categoryId)
    {
        if (ModelState.IsValid)
        {
            _menuService.deleteCategory(categoryId);
            TempData["ToastrMessage"] = "category deleted Successfully";
            TempData["ToastrType"] = "success";
        }
        TempData["ToastrMessage"] = "Error occured while deleting category";
        TempData["ToastrType"] = "error";
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

    // [HttpPost]
    // public IActionResult AddItem(ItemModel model)
    // {
    //     _itemService.getItemsForcategory(model.i.Categoryid, model);
    //     if (ModelState.IsValid)
    //     {
    //         var req = HttpContext.Request;
    //         string email = _cookieService.getValueFromCookie("username", req);
    //         _itemService.addItem(model.i, email);
    //         return PartialView("Menu");
    //     }
    //     else
    //     {
    //         return PartialView("_menuPartial3", model);
    //     }
    // }

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
        _itemService.getItemsForcategory(1,model);
        return PartialView("_add_edititem",model);
    }
    [HttpPost]
    public IActionResult AddNewItem(ItemModel model)
    {
        var req = HttpContext.Request;
        string email = _cookieService.getValueFromCookie("username", req);
        _itemService.addItem(model.i, email);
        return RedirectToAction("ItemsData",new {categoryId = model.i.Categoryid});
    }

    public IActionResult EditItem(int itemid){
        ItemModel model = new ItemModel();
        model.i = _itemService.getItemFromId(itemid);
         _itemService.getItemsForcategory(1,model);
         return PartialView("_add_edititem",model);
    }
}
