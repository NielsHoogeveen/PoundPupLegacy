using Microsoft.AspNetCore.Mvc;

namespace PoundPupLegacy.Controllers;

[Route("blog_post")]
public sealed class BlogPostController : Controller
{
    [HttpGet("create")]
    public IActionResult Index()
    {
        return View("BlogPostCreator");
    }
}
