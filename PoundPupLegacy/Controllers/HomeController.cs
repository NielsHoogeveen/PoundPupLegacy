using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using PoundPupLegacy.ViewModel.Models;
using System.Diagnostics;
using System.Security.Claims;
using IAuthenticationService = PoundPupLegacy.Services.IAuthenticationService;

namespace PoundPupLegacy.Controllers;

public sealed class HomeController : Controller
{
    private readonly IAuthenticationService _authenticationService;

    public HomeController(
        IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
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
        var claimsIdentity = await _authenticationService.Login(userName: user, password: password);
        if (claimsIdentity == null) {
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return new RedirectResult(path);
        }
        var authProperties = new AuthenticationProperties {
            IsPersistent = true,
        };

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties);

        return new RedirectResult(path);
    }

}