
using System.Security.Claims;
using BAL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace pizzashop_n_tier.Controllers;

public class LoginController : Controller
{
    private readonly ILogger<LoginController> _logger;

    private readonly ICookieService _CookieService;

    private readonly ILogin _log;
    private readonly IJwtTokenGenService _jwtTokenGenService;
    private readonly IEmailGenService _emailGenService;
    private readonly IAESService _aesService;

    public LoginController(ILogger<LoginController> logger,IAESService aesService, ICookieService cookieService, ILogin log, IEmailGenService emailGenService, IJwtTokenGenService jwtTokenGenService)
    {
        _logger = logger;
        _CookieService = cookieService;
        _log = log;
        _emailGenService = emailGenService;
        _jwtTokenGenService = jwtTokenGenService;
        _aesService = aesService;
    }
    [HttpGet]
    public IActionResult Index()
    {
        var request = HttpContext.Request;
        if (_CookieService.IsSetCookie(request, "token"))
        {
            var token = _CookieService.getValueFromCookie("token", request);

            var principal = _jwtTokenGenService.ValidateToken(token);

            if (principal != null)
            {
                var role = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
                    Console.WriteLine("Role = "+role);
                if (role == "Chef")
                {
                    return RedirectToAction("getOrderAppPage", "OrderApp");
                }
                else
                { 
                return RedirectToAction("showDashboard", "Dashboard",new{timeid = 1,fromdate = "",endDate = ""});
                }
            }
            else
            {
                Console.WriteLine("Token is expired or invalid.");
                Response.Cookies.Delete("token");
            }
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
            _CookieService.setInCookie(token, res, "token", lgnmdl.IsChecked);
            _CookieService.setInCookie(lgnmdl.username, res, "username", true);
            int userid = _log.getLoggerUId(lgnmdl.username);
            _CookieService.setInCookie(lgnmdl.password, res, "password", true);
            _CookieService.setInCookie(userid.ToString(), res, "userid", true);
            Console.WriteLine("-----");
            TempData["ToastrMessage"] = "Logged in Successfully";
            TempData["ToastrType"] = "success";
            var request = HttpContext.Request;
            // var token1 = _CookieService.getValueFromCookie("token", request);
            var principal = _jwtTokenGenService.ValidateToken(token);

            if (principal != null)
            {
                var role = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
                if (role == "Chef")
                {
                    return RedirectToAction("getOrderAppPage", "OrderApp");
                }
                else
                {
                    return RedirectToAction("showDashboard", "Dashboard",new{timeid = 1,fromdate = "",endDate = ""});
                }
            }

            return RedirectToAction("showDashboard", "Dashboard",new{timeid = 1,fromdate = "",endDate = ""});
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

    public IActionResult AccessDenied()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ForgetPass(string Email)
    {
        if (Email == null)
        {
            ModelState.AddModelError("username", "Enter Your Registered Email Address");
            LoginViewModel model = new LoginViewModel();
            TempData["ToastrMessage"] = "Please enter email address";
            TempData["ToastrType"] = "error";
            return View(model);
        }
        if (!ModelState.IsValid)
        {
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
        else
        {
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
        if (model.confirmpass != model.newpass)
        {
            ModelState.AddModelError("confirmpass", "Password and Confirm Password does not match");
            TempData["ToastrMessage"] = "New password and confirm password does not matched";
            TempData["ToastrType"] = "error";
            return View(model);
        }
        if (!ModelState.IsValid)
        {
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
        // string eMail = _aesService.Decrypt(Email); 

        var model = new PasswordModel
        {
            email = Email,
        };
        return View(model);
    }
}
