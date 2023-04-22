namespace PoundPupLegacy.ViewModel.UI.Services;

public interface IFetchNodeService
{
    [RequireNamedArgs]
    Task<Node?> FetchNode(int id, int userId, int tenantId);
}
