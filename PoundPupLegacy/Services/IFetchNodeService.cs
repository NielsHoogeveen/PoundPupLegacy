using PoundPupLegacy.ViewModel.Models;

namespace PoundPupLegacy.Services;

public interface IFetchNodeService
{
    Task<Node?> FetchNode(int id, int userId, int tenantId);
}
