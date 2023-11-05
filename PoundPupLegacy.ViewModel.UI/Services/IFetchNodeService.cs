namespace PoundPupLegacy.ViewModel.UI.Services;

public interface IFetchNodeService
{
    [RequireNamedArgs]
    Task<Node?> FetchNode(int id, int userId, int tenantId);
}

public interface IFetchNodeService<T>
    where T : class, Node
{
    [RequireNamedArgs]
    Task<T?> FetchNode(int nodeId, int userId, int tenantId);
}
