namespace PoundPupLegacy.ViewModel.UI.Services;

public interface IFetchSubgroupService
{
    [RequireNamedArgs]
    Task<SubgroupPagedList> GetSubGroupPagedList(int userId, int subgroupId, int pageSize, int pageNumber);
}
