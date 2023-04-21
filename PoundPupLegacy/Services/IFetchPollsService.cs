using PoundPupLegacy.Common;
using PoundPupLegacy.ViewModel.Models;

namespace PoundPupLegacy.Services;

public interface IFetchPollsService
{
    [RequireNamedArgs]
    public Task<Polls> GetPolls(int userId, int tenantId, int pageNumber, int pageSize);
}
