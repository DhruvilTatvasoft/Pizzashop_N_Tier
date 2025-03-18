using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BAL.Interfaces;
using DAL.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace pizzashop_n_tier.Controllers;

public class LoginController : Controller
{
    private readonly ILogger<LoginController> _logger;

    private readonly ICookieService _CookieService;

    private readonly ILogin _log;

    private readonly IConfiguration _configuration;

    private readonly IEmailGenService _emailGenService;

    private readonly IAESService _aesservice;

    private readonly IImagePath _imageService;

    public LoginController(ILogger<LoginController> logger,IImagePath imageService, ICookieService cookieService,IAESService AesService, ILogin log, IEmailGenService emailGenService)
    {
        _logger = logger;
        _CookieService = cookieService;
        _log = log;
        _emailGenService = emailGenService;
        _imageService = imageService;
        _aesservice = AesService;
    }
    [HttpGet]
    public IActionResult Index()
    {
        var req = HttpContext.Request;
        if (_CookieService.IsSetCookie(req, "token"))
        {
            Console.WriteLine("OK");
            
           TempData["ToastrMessage"] = "Logged in Successfully";
           TempData["ToastrType"] = "success";
            return RedirectToAction("showDashboard", "Dashboard");
        }
        return View();
    }
    [HttpPost]
    public IActionResult Index(LoginViewModel lgnmdl)
    {
        if (!ModelState.IsValid)
        {
            TempData["ToastrMessage"] = "Some credentials are missing";
           TempData["ToastrType"] = "error";
            return View(lgnmdl);
        }
        Console.WriteLine(_log.checkloggerInDb(lgnmdl));
        if (_log.checkloggerInDb(lgnmdl))
        {

            var token = _log.saveLogger(lgnmdl);
            var res = HttpContext.Response;
            _CookieService.setInCookie(token, res, "token",lgnmdl.IsChecked);
            _CookieService.setInCookie(lgnmdl.username, res, "username",true);
            int userid = _log.getLoggerUId(lgnmdl.username);
            _CookieService.setInCookie(lgnmdl.password, res, "password",true);
            _CookieService.setInCookie(userid.ToString(), res, "userid",true);
            // string imagePath = _imageService.getImagePathFromUid(userid);
            // _CookieService.setInCookie(imagePath,res,"userImage",true);
            Console.WriteLine("-----");
            TempData["ToastrMessage"] = "Logged in Successfully";
           TempData["ToastrType"] = "success";
            return RedirectToAction("showDashboard", "Dashboard");
        }
        else
        {
            ModelState.AddModelError("username", "Invalid Email or Password");
            ModelState.AddModelError("password", "Invalid Email or Password");
            TempData["ToastrMessage"] = "invalid username and password";
            TempData["ToastrType"] = "error";
            return View(lgnmdl);
        }

    }

    [HttpPost]
    public async Task<IActionResult> ForgetPass(string Email)
    {
        if(Email == null){
             ModelState.AddModelError("username", "Enter Your Registered Email Address");
            LoginViewModel model = new LoginViewModel();
            TempData["ToastrMessage"] = "Please enter email address";
            TempData["ToastrType"] = "error";
             return View(model);
        }
        if(!ModelState.IsValid){
            LoginViewModel model = new LoginViewModel();
            TempData["ToastrMessage"] = "Invalid email address";
            TempData["ToastrType"] = "error";
            return View(model);
        }
        if (_log.emailExist(Email))
        {
            Console.WriteLine("email generated");
            var req = HttpContext.Request;
            _emailGenService.generateEmail(req, Email);
            TempData["ToastrMessage"] = "Email sended";
            TempData["ToastrType"] = "success";
        }
        else{
             ModelState.AddModelError("Username", "Email does not exist !! Register First !!");
        }
        return View();
    }
    [HttpGet]
    public IActionResult ForgetPass()
    {
        
        return View();
    }
    [HttpPost]
    public IActionResult ResetPass(PasswordModel model)
    {
        if(model.confirmpass != model.newpass){
            ModelState.AddModelError("confirmpass", "Password and Confirm Password does not match");
            TempData["ToastrMessage"] = "New password and confirm password does not matched";
           TempData["ToastrType"] = "error";
            return View(model);
        }
        if(!ModelState.IsValid){
            TempData["ToastrMessage"] = "Please enter password";
           TempData["ToastrType"] = "error";
            return View(model);
        }
        _log.updatePass(model);
        TempData["ToastrMessage"] = "Password updated Successfully";
           TempData["ToastrType"] = "success";
        return View("Index");
    }
    [HttpGet]
    public IActionResult ResetPass()
    {
        string Email = HttpContext.Request.Query["email"];
        // string eMail = _aesservice.Decrypt(Email); 
        var model = new PasswordModel
        {
            email = Email,
        };
        return View(model);
    }
}
