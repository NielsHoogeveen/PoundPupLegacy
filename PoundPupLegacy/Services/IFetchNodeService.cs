using PoundPupLegacy.Common;
using PoundPupLegacy.ViewModel.Models;

namespace PoundPupLegacy.Services;

public interface IFetchNodeService
{
    [RequireNamedArgs]
    Task<Node?> FetchNode(int id, int userId, int tenantId);
}
