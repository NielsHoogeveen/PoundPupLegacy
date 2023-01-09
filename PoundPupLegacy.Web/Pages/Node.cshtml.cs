using Microsoft.AspNetCore.Mvc.RazorPages;
using PoundPupLegacy.ViewModel;
using PoundPupLegacy.Web.Services;
using System.Diagnostics;

namespace PoundPupLegacy.Web.Pages;

public class NodeModel : PageModel
{
    public Node? Node { get; set; }

    private FetchNodeService _fetchNodeService;
    private readonly ILogger<NodeModel> _logger;
    public NodeModel(ILogger<NodeModel> logger, FetchNodeService fetchNodeService)
    {
        _logger = logger;
        _fetchNodeService = fetchNodeService;
    }

    public async Task OnGet(int id)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        Node = await _fetchNodeService.FetchNode(id);
        _logger.LogInformation($"Fetched node {id} in {stopwatch.Elapsed.TotalMilliseconds} ms");

    }
}
