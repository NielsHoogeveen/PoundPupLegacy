using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using PoundPupLegacy.Models;
using PoundPupLegacy.Services;
using System.Diagnostics;
using System.Security.Claims;
using IAuthenticationService = PoundPupLegacy.Services.IAuthenticationService;

namespace PoundPupLegacy.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IAuthenticationService _authenticationService;
    private readonly ISiteDataService _siteDataService;
    private readonly INodeCacheService _nodeCacheService;
    private readonly ICongressionalDataService _congressionalDataService;

    public HomeController(
        ILogger<HomeController> logger,
        IAuthenticationService authenticationService,
        ISiteDataService siteDataService,
        INodeCacheService nodeCacheService,
        ICongressionalDataService congressionalDataService)
    {
        _logger = logger;
        _authenticationService = authenticationService;
        _siteDataService = siteDataService;
        _nodeCacheService = nodeCacheService;
        _congressionalDataService = congressionalDataService;
    }

    public IActionResult Index()
    {
        var bla = HttpContext.Items["UserMenu"];
        return View();
    }

    public async Task<IActionResult> AllElse()
    {
        var stopwatch = new Stopwatch();
        if (Request.Path == "/NotFound")
        {
            return View("NotFound");
        }
        
        stopwatch.Start();

        var congressionalMeetingChamber = await _congressionalDataService.GetCongressionalMeetingChamberResult();
        if(congressionalMeetingChamber is not null) 
        { 
            return new ContentResult {
                Content = congressionalMeetingChamber,
                ContentType = "text/html"
            };
        }
        var urlId = _siteDataService.GetIdForUrlPath();
        if (urlId is null)
        {
            return NotFound();
        }
        var id = urlId.Value;
        var result = await _nodeCacheService.GetResult(id);
        _logger.LogInformation($"Fetched node {id} in {stopwatch.Elapsed.TotalMilliseconds} ms");
        return result;
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var referer = this.HttpContext.Request.Headers.Referer.ToString();
        var path = referer.Substring(referer.IndexOf("/", 10));

        // Clear the existing external cookie
        await HttpContext.SignOutAsync(
            CookieAuthenticationDefaults.AuthenticationScheme);

        return new RedirectResult(path);
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login(string user, string password)
    {
        var referer = this.HttpContext.Request.Headers.Referer.ToString();
        var path = referer.Substring(referer.IndexOf("/", 10));
        var claimsIdentity = await _authenticationService.Login(user, password);
        if (claimsIdentity == null)
        {
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return new RedirectResult(path);
        }
        var authProperties = new AuthenticationProperties
        {
            IsPersistent = true,
        };

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties);

        return new RedirectResult(path);
    }

}