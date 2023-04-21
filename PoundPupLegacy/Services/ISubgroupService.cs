using PoundPupLegacy.Common;
using PoundPupLegacy.ViewModel.Models;

namespace PoundPupLegacy.Services;

public interface ISubgroupService
{
    [RequireNamedArgs]
    Task<SubgroupPagedList> GetSubGroupPagedList(int userId, int subgroupId, int pageSize, int pageNumber);
}
