using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;

namespace PoundPupLegacy.Services.Implementation;

internal class NodeCacheService : INodeCacheService
{
    private readonly ISiteDataService _siteDataService;
    private readonly IConfiguration _configuration;
    private readonly IFetchNodeService _fetchNodeService;
    private readonly IRazorViewToStringService _razorViewToStringService;
    public NodeCacheService(
        ISiteDataService siteDataService,
        IConfiguration configuration,
        IFetchNodeService fetchNodeService,
        IRazorViewToStringService razorViewToStringService)
    {
        _siteDataService = siteDataService;
        _configuration = configuration;
        _fetchNodeService = fetchNodeService;
        _razorViewToStringService = razorViewToStringService;

    }

    private record struct TenantNode
    {
        public required int TenantId { get; init; }
        public required int NodeId { get; init; }
    }
    private readonly ConcurrentDictionary<TenantNode, string> _nodeCache = new ConcurrentDictionary<TenantNode, string>();

    public void Remove(int nodeId)
    {
        var tenantNode = new TenantNode { NodeId = nodeId, TenantId = _siteDataService.GetTenantId() };
        _nodeCache.TryRemove(tenantNode, out _);
    }

    public async Task<IActionResult> GetResult(int nodeId)
    {
        var tenantNode = new TenantNode { NodeId = nodeId, TenantId = _siteDataService.GetTenantId() };
        if (_configuration["NodeCaching"] != "on") {
            return await AssembleNewResponse(nodeId, tenantNode, false);
        }
        if (_nodeCache.TryGetValue(tenantNode, out var html)) {
            return new ContentResult {
                Content = html,
                ContentType = "text/html"
            };
        }
        else {
            return await AssembleNewResponse(nodeId, tenantNode, true);
        }
    }

    private async Task<string?> GetNodeString(int id)
    {
        var node = await _fetchNodeService.FetchNode(id);
        if (node == null) {
            return null;
        }
        var html = await _razorViewToStringService.GetFromView("/Views/Shared/Node.cshtml", node);
        return html;
    }

    private async Task<IActionResult> AssembleNewResponse(int nodeId, TenantNode tenantNode, bool storeInDictionary)
    {
        var nodeString = await GetNodeString(nodeId);
        if (nodeString != null) {
            if (storeInDictionary) {
                _nodeCache[tenantNode] = nodeString;
            }

            return new ContentResult {
                Content = nodeString,
                ContentType = "text/html"
            };
        }
        else {
            return new NotFoundResult();
        }
    }
}
