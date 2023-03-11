using PoundPupLegacy.ViewModel;

namespace PoundPupLegacy.Services;

public interface IFetchPollsService
{
    public Task<Polls> GetPolls(int userId, int tenantId, int startIndex, int length);
}
