using PoundPupLegacy.ViewModel;

namespace PoundPupLegacy.Services;

public interface ISubgroupService
{
    Task<SubGroupPagedList?> GetSubGroupPagedList(int userId, int subgroupId, int limit, int offset);
}
