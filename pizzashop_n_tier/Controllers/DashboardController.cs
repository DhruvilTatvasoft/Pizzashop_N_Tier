using System.Reflection.Metadata.Ecma335;
using AspNetCoreGeneratedDocument;
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
    public IActionResult updateProfile(UserDetailModel model)
    {
        // if(model.firstname == null || model.lastname == null ||model.username == null ||  model.Phone == null || model.country == null || model.state == null || model.address == null || model.Zipcode == null ){
        if(ModelState.IsValid ){ 
        var req = HttpContext.Request;
        string email = _cookieService.getValueFromCookie("username", req);
        Console.WriteLine("in update profile");
        _user.updateUser(model, email);
        return View("Myprofile", model);
        }  
        else{
            return View("Myprofile",model);
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
            return RedirectToAction("index", "LoginController");
        }
        else{
             ModelState.AddModelError("oldpass", "Please enter correct Password");
             return View(model);
        }
    }
    public IActionResult Logout()
    {
        HttpContext.Response.Cookies.Delete("token");
        HttpContext.Response.Cookies.Delete("username");
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
        var req = HttpContext.Request;
        string email = _cookieService.getValueFromCookie("username", req);
        Console.WriteLine("in post method");
        _user.saveNewUser(model, email);
        _emailService.emailForForgetPass(req, model.email, model.password);
        return View("showUsers");
    }
    public IActionResult getUsers(string search, int maxRows = 1, int currentPage = 1)
    {
        userpagingdetailmodel model = new userpagingdetailmodel();
        model = _user.loadusers(model, currentPage, maxRows, search);
        return PartialView("_TablePartialView", model);
    }
    public IActionResult getSearchedUser(string search, int maxRows = 1, int currentPage = 1)
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
        return View("getUsers");
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
        _user.updateUser(model, Id);
        return View("showUsers");
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
        var req = HttpContext.Request;
        string email = _cookieService.getValueFromCookie("username", req);
        _menuService.addNewcategory(model, email);
        return View("Menu", model);
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
        var req = HttpContext.Request;
        string email = _cookieService.getValueFromCookie("username", req);
        _menuService.editCategory(model, email);
        return RedirectToAction("Menu");
    }
    public IActionResult DeleteCategory(int categoryId)
    {
        _menuService.deleteCategory(categoryId);
        return View("Menu");
    }

    [HttpPost]
    public IActionResult AddItem(ItemModel model)
    {
        var req = HttpContext.Request;
        string email = _cookieService.getValueFromCookie("username", req);
        _itemService.addItem(model.i, email);
        return PartialView("Menu");
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


}
