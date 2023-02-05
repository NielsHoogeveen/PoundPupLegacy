using Microsoft.AspNetCore.Mvc;
using PoundPupLegacy.Services;
using System.Diagnostics;

namespace PoundPupLegacy.Web.Controllers;

[Route("node")]
public class NodeController : Controller
{
    private readonly ILogger<NodeController> _logger;
    private readonly FetchNodeService _fetchNodeService;
    private readonly SiteDataService _siteDataService;

    public NodeController(
        ILogger<NodeController> logger, 
        FetchNodeService fetchNodeService, 
        SiteDataService siteDataService) 
    {
        _logger = logger;
        _fetchNodeService = fetchNodeService;
        _siteDataService = siteDataService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetNode(int id)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        var tenantId = _siteDataService.GetTenantId(HttpContext);
        var urlPath = _siteDataService.GetUrlPathForId(tenantId, id);
        if (urlPath is not null)
        {
            return Redirect($"/{urlPath}");
        }
        var node = await _fetchNodeService.FetchNode(id, HttpContext);
        if(node == null)
        {
            return NotFound();
        }
        _logger.LogInformation($"Fetched node {id} in {stopwatch.Elapsed.TotalMilliseconds} ms");

        return View("Node",node);
    }

}
