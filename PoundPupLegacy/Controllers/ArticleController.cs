using Microsoft.AspNetCore.Mvc;

namespace PoundPupLegacy.Controllers;

[Route("article")]
public class ArticleController : Controller
{
    [HttpGet("create")]
    public IActionResult Index()
    {
        return View("ArticleCreator");
    }
}
