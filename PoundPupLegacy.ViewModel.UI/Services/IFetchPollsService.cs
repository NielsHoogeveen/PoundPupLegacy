namespace PoundPupLegacy.ViewModel.UI.Services;

public interface IFetchPollsService
{
    [RequireNamedArgs]
    public Task<Polls> GetPolls(int userId, int tenantId, int pageSize, int pageNumber);
}
