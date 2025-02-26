using AspNetCoreGeneratedDocument;
using Microsoft.AspNetCore.Mvc;

public class DashboardController : Controller{
    
    public readonly ILogin _log;

    public readonly IUser _user;

    public readonly ICookieService _cookieService;
    public DashboardController(ILogin log,IUser user,ICookieService cookieService){
        _log = log;
        _user = user;
        _cookieService = cookieService;
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
    public IActionResult Logout(){
        HttpContext.Response.Cookies.Delete("token");
        HttpContext.Response.Cookies.Delete("username");
        return RedirectToAction("Index","Login");
    }
    public IActionResult AddUser(){
        return View();
    }
    public IActionResult getUsers(){
        return View();
    }
    public IActionResult getSearchedUser(){
        return View();
    }
    public IActionResult showUsers(){
        return View();
    }
}