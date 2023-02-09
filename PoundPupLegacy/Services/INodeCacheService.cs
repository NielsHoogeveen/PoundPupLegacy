using Microsoft.AspNetCore.Mvc;

namespace PoundPupLegacy.Services;

public interface INodeCacheService
{
    public Task<IActionResult> GetResult(HttpContext context, int nodeId);
}
