using Microsoft.AspNetCore.Mvc;
using PoundPupLegacy.Services;

namespace PoundPupLegacy.Controllers;

[Route("blogs")]
public sealed class BlogsController : Controller
{
    private readonly IFetchBlogsService _fetchBlogsService;
    private readonly ISiteDataService _siteDataService;
    private readonly IUserService _userService;
    public BlogsController(
        IFetchBlogsService fetchBlogsService,
        ISiteDataService siteDataService,
        IUserService userService)
    {
        _fetchBlogsService = fetchBlogsService;
        _siteDataService = siteDataService;
        _userService = userService;
    }
    public async Task<IActionResult> Index()
    {
        var userId = _userService.GetUserId(HttpContext.User);
        var tenantId = _siteDataService.GetTenantId(Request);
        if (!_siteDataService.HasAccess(userId, tenantId, Request)) {
            return NotFound();
        }
        var model = await _fetchBlogsService.FetchBlogs(tenantId);
        return View("Blogs", model);
    }
}
