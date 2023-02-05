using Microsoft.AspNetCore.Mvc;
using PoundPupLegacy.Web.Services;

namespace PoundPupLegacy.Controllers;

[Route("blogs")]
public class BlogsController : Controller
{
    FetchBlogsService _fetchBlogsService;
    public BlogsController(FetchBlogsService fetchBlogsService)
    {
        _fetchBlogsService = fetchBlogsService;
    }
    public async Task<IActionResult> Index()
    {
        var model = await _fetchBlogsService.FetchBlogs(HttpContext);
        return View("Blogs",model);
    }
}
