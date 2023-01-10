using Microsoft.AspNetCore.Mvc;
using PoundPupLegacy.Web.Services;
using System.Diagnostics;

namespace PoundPupLegacy.Web.Controllers;

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
    public async Task<IActionResult> Index(int id)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        var node = await _fetchNodeService.FetchNode(id);
        _logger.LogInformation($"Fetched node {id} in {stopwatch.Elapsed.TotalMilliseconds} ms");

        return View(node);
    }
}
