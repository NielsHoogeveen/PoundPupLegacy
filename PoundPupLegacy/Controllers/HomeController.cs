using Microsoft.AspNetCore.Mvc;
using PoundPupLegacy.ViewModel.Models;
using System.Diagnostics;

namespace PoundPupLegacy.Controllers;

public sealed class HomeController : Controller
{
    public HomeController()
    {
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    [HttpPost("logout")]
    public IActionResult Logout()
    {
        var referer = this.HttpContext.Request.Headers.Referer.ToString();
        var path = referer.Substring(referer.IndexOf("/", 10));

        return new RedirectResult(path);
    }
}