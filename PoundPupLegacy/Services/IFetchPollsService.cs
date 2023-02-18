using PoundPupLegacy.ViewModel;

namespace PoundPupLegacy.Services;

public interface IFetchPollsService
{
    public Task<Polls> GetPolls(int startIndex, int length);
}
