using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;

namespace PoundPupLegacy.Services.Implementation;

internal class NodeCacheService : INodeCacheService
{
    private readonly IConfiguration _configuration;
    private readonly IFetchNodeService _fetchNodeService;
    private readonly IRazorViewToStringService _razorViewToStringService;
    public NodeCacheService(
        IConfiguration configuration,
        IFetchNodeService fetchNodeService,
        IRazorViewToStringService razorViewToStringService)
    {
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

    public void Remove(int nodeId, int tenantId)
    {
        var tenantNode = new TenantNode { NodeId = nodeId, TenantId = tenantId };
        _nodeCache.TryRemove(tenantNode, out _);
    }

    public async Task<IActionResult> GetResult(int nodeId, int userId, int tenantId, HttpContext context)
    {
        var tenantNode = new TenantNode { NodeId = nodeId, TenantId = tenantId };
        if (_configuration["NodeCaching"] != "on") {
            return await AssembleNewResponse(nodeId, userId, tenantId, context, tenantNode, false);
        }
        if (_nodeCache.TryGetValue(tenantNode, out var html)) {
            return new ContentResult {
                Content = html,
                ContentType = "text/html"
            };
        }
        else {
            return await AssembleNewResponse(nodeId, userId, tenantId, context, tenantNode, true);
        }
    }

    private async Task<string?> GetNodeString(int id, int userId, int tenantId, HttpContext context)
    {
        var node = await _fetchNodeService.FetchNode(id, userId, tenantId);
        if (node == null) {
            return null;
        }
        var html = await _razorViewToStringService.GetFromView("/Views/Shared/Node.cshtml", node, context);
        return html;
    }

    private async Task<IActionResult> AssembleNewResponse(int nodeId, int userId, int tenantId, HttpContext context, TenantNode tenantNode, bool storeInDictionary)
    {
        var nodeString = await GetNodeString(nodeId, userId, tenantId, context);
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
