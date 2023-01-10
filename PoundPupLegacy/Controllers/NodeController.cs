using Microsoft.AspNetCore.Mvc;
using PoundPupLegacy.Services;
using System.Diagnostics;

namespace PoundPupLegacy.Web.Controllers;

[Route("node")]
public class NodeController : Controller
{
    private readonly ILogger<NodeController> _logger;
    private readonly FetchNodeService _fetchNodeService;

    public NodeController(
        ILogger<NodeController> logger, 
        FetchNodeService fetchNodeService) 
    {
        _logger = logger;
        _fetchNodeService = fetchNodeService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetNode(int id)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        var node = await _fetchNodeService.FetchNode(id);
        _logger.LogInformation($"Fetched node {id} in {stopwatch.Elapsed.TotalMilliseconds} ms");

        return View("Node",node);
    }
}
