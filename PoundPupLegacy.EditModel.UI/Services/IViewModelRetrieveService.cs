namespace PoundPupLegacy.EditModel.UI.Services;

public interface IViewModelRetrieveService<TViewModel>
    where TViewModel: class, Node
{
    [RequireNamedArgs]
    Task<TViewModel?> GetViewModelAsync(int nodeId, int userId, int tenantId);

    [RequireNamedArgs]
    Task<TViewModel?> GetViewModelAsync(int userId, int tenantId);
}
