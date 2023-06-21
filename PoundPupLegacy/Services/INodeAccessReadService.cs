using PoundPupLegacy.Models;

namespace PoundPupLegacy.Services;

public interface INodeAccessReadService
{
    public Task<List<NodeAccess>> ReadNodeAccess(int nodeId);
}
