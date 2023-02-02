using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using PoundPupLegacy.Models;
using PoundPupLegacy.Services;
using System.Diagnostics;
using System.Security.Claims;

namespace PoundPupLegacy.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly Services.AuthenticationService _authenticationService;
    private readonly FetchNodeService _fetchNodeService;
    private readonly SiteDataService _siteDataService;
    private readonly RazorViewToStringService _viewService;

    public HomeController(ILogger<HomeController> logger, 
        Services.AuthenticationService authenticationService,
        FetchNodeService fetchNodeService,
        SiteDataService siteDataService,
        RazorViewToStringService viewService)
    {
        _logger = logger;
        _authenticationService = authenticationService;
        _fetchNodeService = fetchNodeService;
        _siteDataService = siteDataService;
        _viewService = viewService;
    }

    public IActionResult Index()
    {
        var bla = HttpContext.Items["UserMenu"];
        return View();
    }

    public async Task<IActionResult> AllElse()
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        
        var tenantId = _siteDataService.GetTenantId(this.HttpContext.Request.Host.Value);
        if (tenantId is null)
        {
            return NotFound();
        }
        var urlId = _siteDataService.GetIdForUrlPath(tenantId.Value, this.HttpContext.Request.Path.Value!.Substring(1));
        if (urlId is null)
        {
            return NotFound();
        }
        var node = await _fetchNodeService.FetchNode(urlId.Value, HttpContext.User);
        if (node == null)
        {
            return NotFound();
        }
        _logger.LogInformation($"Fetched node {urlId} in {stopwatch.Elapsed.TotalMilliseconds} ms");

        return View("/Views/Node/Node.cshtml", node);
    }
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    [HttpPost("logout")]
    public async Task<IActionResult> OnGetAsync()
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

        var cp = new ClaimsPrincipal(claimsIdentity);
        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties);

        var u = HttpContext.User;
        return new RedirectResult(path);
    }
}