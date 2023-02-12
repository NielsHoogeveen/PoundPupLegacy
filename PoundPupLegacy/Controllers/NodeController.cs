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

    public NodeController(
        ILogger<NodeController> logger,
        ISiteDataService siteDataService,
        INodeCacheService nodeCacheService)
    {
        _logger = logger;
        _siteDataService = siteDataService;
        _nodeCacheService = nodeCacheService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetNode(int id)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        var tenantId = _siteDataService.GetTenantId();
        var urlPath = _siteDataService.GetUrlPathForId(tenantId, id);
        if (urlPath is not null)
        {
            return Redirect($"/{urlPath}");
        }
        var result = await _nodeCacheService.GetResult(id);
        _logger.LogInformation($"Fetched node {id} in {stopwatch.Elapsed.TotalMilliseconds} ms");
        return result;
    }
}
