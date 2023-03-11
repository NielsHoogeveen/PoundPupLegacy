using Microsoft.AspNetCore.Mvc;
using PoundPupLegacy.Services;
using System.Diagnostics;

namespace PoundPupLegacy.Web.Controllers;

[Route("node")]
public class NodeController : Controller
{
    private readonly ILogger<NodeController> _logger;
    private readonly ISiteDataService _siteDataService;
    private readonly INodeCacheService _nodeCacheService;
    private readonly IUserService _userService;

    public NodeController(
        ILogger<NodeController> logger,
        ISiteDataService siteDataService,
        INodeCacheService nodeCacheService,
        IUserService userService)
    {
        _logger = logger;
        _siteDataService = siteDataService;
        _nodeCacheService = nodeCacheService;
        _userService = userService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetNode(int id)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        var tenantId = _siteDataService.GetTenantId(Request);
        var urlPath = _siteDataService.GetUrlPathForId(tenantId, id);
        if (urlPath is not null) {
            return Redirect($"/{urlPath}");
        }
        var userId = _userService.GetUserId(HttpContext.User);
        var result = await _nodeCacheService.GetResult(id, userId, tenantId);
        _logger.LogInformation($"Fetched node {id} in {stopwatch.Elapsed.TotalMilliseconds} ms");
        return result;
    }
}
