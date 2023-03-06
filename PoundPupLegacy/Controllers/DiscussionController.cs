using Microsoft.AspNetCore.Mvc;

namespace PoundPupLegacy.Controllers;

[Route("discussion")]
public class DiscussionController : Controller
{
    [HttpGet("create")]
    public IActionResult Index()
    {
        return View("DiscussionCreator");
    }
}
