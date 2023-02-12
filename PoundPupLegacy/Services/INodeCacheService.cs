using Microsoft.AspNetCore.Mvc;

namespace PoundPupLegacy.Services;

public interface INodeCacheService
{
    public Task<IActionResult> GetResult(int nodeId);

    void Remove(int nodeId);
}
