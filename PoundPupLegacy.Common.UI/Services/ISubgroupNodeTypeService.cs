using PoundPupLegacy.Common;

namespace PoundPupLegacy.Services;

public interface ISubgroupNodeTypeService
{
    Task<List<CreateableNodeTypes>> GetCreateableNodeTypes(int subgroupId, int userId);
}
