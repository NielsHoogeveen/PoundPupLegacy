using Microsoft.AspNetCore.Mvc;
using PoundPupLegacy.Services;

namespace PoundPupLegacy.Controllers;

[Route("blogs")]
public class BlogsController : Controller
{
    private readonly IFetchBlogsService _fetchBlogsService;
    private readonly ISiteDataService _siteDataService;
    public BlogsController(IFetchBlogsService fetchBlogsService, ISiteDataService siteDataService)
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
