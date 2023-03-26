using PoundPupLegacy.ViewModel;

namespace PoundPupLegacy.Services;

public interface ISubgroupService
{
    Task<SubgroupPagedList?> GetSubGroupPagedList(int userId, int subgroupId, int limit, int offset);
}
