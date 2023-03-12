using Microsoft.AspNetCore.Mvc;

namespace PoundPupLegacy.Services;

public interface INodeCacheService
{
    public Task<IActionResult> GetResult(int nodeId, int userId, int tenantId, HttpContext context);

    void Remove(int nodeId, int tenantId);
}
