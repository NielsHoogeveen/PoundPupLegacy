using Microsoft.AspNetCore.Mvc;
using PoundPupLegacy.Services;
using PoundPupLegacy.Web.Services;

namespace PoundPupLegacy.Controllers;

[Route("blogs")]
public class BlogsController : Controller
{
    private readonly FetchBlogsService _fetchBlogsService;
    private readonly SiteDataService _siteDataService;
    public BlogsController(FetchBlogsService fetchBlogsService, SiteDataService siteDataService)
    {
        _fetchBlogsService = fetchBlogsService;
        _siteDataService = siteDataService;
    }
    public async Task<IActionResult> Index()
    {
        if (!_siteDataService.HasAccess(HttpContext))
        {
            return NotFound();
        }
        var model = await _fetchBlogsService.FetchBlogs(HttpContext);
        return View("Blogs",model);
    }
}
