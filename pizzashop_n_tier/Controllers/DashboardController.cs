using System.Reflection.Metadata.Ecma335;
using AspNetCoreGeneratedDocument;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

public class DashboardController : Controller{
    
    public readonly ILogin _log;

    public readonly IUser _user;

    public readonly ICookieService _cookieService;

    public readonly IEmailGenService _emailService;

    public readonly IMenuService    _menuService;
    public DashboardController(ILogin log,IUser user,ICookieService cookieService,IEmailGenService emailService,IMenuService menuService){
        _log = log;
        _user = user;
        _cookieService = cookieService;
        _emailService = emailService;
        _menuService = menuService;
    }
    public IActionResult ShowDashboard(){
        return View();
    }
    public IActionResult Myprofile(){
        var req = HttpContext.Request;
        string email = _cookieService.getValueFromCookie("username",req);
        // Console.WriteLine("email from session" + email);
        var user = _log.getUser(email);
        UserDetailModel m = _log.setUserInModel(user);
        return View(m);
    }
    [HttpPost]
    public IActionResult updateProfile(UserDetailModel model){
        var req = HttpContext.Request;
        string email = _cookieService.getValueFromCookie("username",req);
        Console.WriteLine("in update profile");
            _user.updateUser(model,email);
            return View("Myprofile",model);
    }
    [HttpGet]
    public IActionResult ResetPassword(){
        var req = HttpContext.Request; 
        var model = new chang_p_model{
            oldpass = _user.getPass(_cookieService.getValueFromCookie("username",req))
        };
        return View(model);
    }

    [HttpPost]
    public IActionResult ResetPassword(chang_p_model model){
        var req = HttpContext.Request;
        string email = _cookieService.getValueFromCookie("username",req);
        _user.changePass(req,model,email);
        return RedirectToAction("ShowDashboard", "Dashboard");
    }
    public IActionResult Logout(){
        HttpContext.Response.Cookies.Delete("token");
        HttpContext.Response.Cookies.Delete("username");
        return RedirectToAction("Index","Login");
    }

    [HttpGet]
    public IActionResult AddUser(){
        UserDetailModel model = new UserDetailModel();
        model.Role = _user.getRoles();
        model.Country = _user.getAllCountries();
        return View(model);
    }
    [HttpPost]
    public IActionResult AddUser(UserDetailModel model){
        var req = HttpContext.Request;
        string email = _cookieService.getValueFromCookie("username",req);
        Console.WriteLine("in post method");
        _user.saveNewUser(model,email);
        _emailService.emailForForgetPass(req,model.email,model.password);
        return View("showUsers");
    }
    public IActionResult getUsers(string search,int currentPage = 1){
        userpagingdetailmodel model = new userpagingdetailmodel();  
        model = _user.loadusers(model,currentPage,5,search);
        return PartialView("_TablePartialView",model);
    }
    public IActionResult getSearchedUser(string search,int currentPage = 1){
        userpagingdetailmodel model = new userpagingdetailmodel();  
        Console.WriteLine("searching user");
        model = _user.loadusers(model,currentPage,5,search);
        Console.WriteLine("searched");
        return PartialView("_TablePartialView",model);
    }

     [HttpGet]
        public IActionResult GetStates(int countryId)
        {
            var states = _user.getStates(countryId);
            return Json(new SelectList(states, "Stateid", "Statename"));
        }
   [HttpGet]
        public IActionResult DeleteUser(int Id){
            _user.deleteUser(Id);
            return View("getUsers");
        }
[HttpGet]
        public IActionResult EditUser(int Id){
            UserDetailModel model = new UserDetailModel();
            Console.WriteLine("In edit user");
          model =  _user.getUserDetails(Id);
            return View(model);
        }
[HttpPost]
public IActionResult EditUser(UserDetailModel model,int Id){
    _user.updateUser(model,Id);
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
        public IActionResult Roles(){
            RolesModel model = new RolesModel();
            model.Role = _user.getAllRoles();
            return View(model);
        }
    public IActionResult showUsers(){
        return View();
    }

    [HttpGet]
    public IActionResult PermissionsOfRole(int Id){
    //     PermissionsModel model = new PermissionsModel();
    //     model = _user.permissionsForRole(Id);
    //     return View("permissions",model);
     PermissionsModel2 model = new PermissionsModel2();
        model.roleid = Id;
        model = _user.permissionsForRole(Id);
        return View("permissions",model);
    }

// [HttpGet]
//     public IActionResult PermissionOfRole(int Id){
       
//     }
    [HttpPost]
    public IActionResult UpdatePermissions(PermissionsModel model){
        _user.updatePermissions(model);
         return RedirectToAction("permissionsOfRole", "Dashboard");
    }


    [HttpGet]
    public IActionResult Menu(){
        MenuModel model = new MenuModel();
         _menuService.GetCategories(model);
        return View(model);
    }

    public IActionResult Addcategory(MenuModel model){
         var req = HttpContext.Request;
        string email = _cookieService.getValueFromCookie("username",req);
        _menuService.addNewcategory(model,email);
        return View("Menu",model);
    }

}  
