using DAL.Data;
using Microsoft.AspNetCore.Mvc;

namespace pizzashop_n_tier.Controllers;

public class LoginController : Controller
{
    private readonly ILogger<LoginController> _logger;

    private readonly ICookieService _CookieService;

    private readonly ILogin _log;

    private readonly IConfiguration _configuration;

    private readonly IEmailGenService _emailGenService;

    public LoginController(ILogger<LoginController> logger, ICookieService cookieService, ILogin log, IEmailGenService emailGenService)
    {
        _logger = logger;
        _CookieService = cookieService;
        _log = log;
        _emailGenService = emailGenService;
    }
    [HttpGet]
    public IActionResult Index()
    {
        var req = HttpContext.Request;
        if (_CookieService.IsSetCookie(req, "token"))
        {
            Console.WriteLine("OK");
            return RedirectToAction("showDashboard", "Dashboard");
        }
        return View();
    }

    [HttpPost]
    public IActionResult Index(LoginViewModel lgnmdl)
    {
        Console.WriteLine(_log.checkloggerInDb(lgnmdl));
        if (_log.checkloggerInDb(lgnmdl))
        {
            var token = _log.saveLogger(lgnmdl);
            var res = HttpContext.Response;
            if (lgnmdl.IsChecked)
            {
                _CookieService.setInCookie(token, res, "token");
            }
            _CookieService.setInCookie(lgnmdl.username, res, "username");
            Console.WriteLine("-----");
            return RedirectToAction("showDashboard", "Dashboard");
        }
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ForgetPass(string Email)
    {
        if (_log.emailExist(Email))
        {
            Console.WriteLine("email generated");
            var req = HttpContext.Request;
            _emailGenService.generateEmail(req, Email);
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
        _log.updatePass(model);
        return View("Index");
    }
    [HttpGet]
    public IActionResult ResetPass()
    {
        string Email = HttpContext.Request.Query["email"];
        var model = new PasswordModel
        {
            email = Email
        };
        return View(model);
    }
}
