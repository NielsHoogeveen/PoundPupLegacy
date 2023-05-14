namespace PoundPupLegacy.EditModel.UI.Services;

public interface IEditService<T>
{
    [RequireNamedArgs]
    Task<T?> GetViewModelAsync(int urlId, int userId, int tenantId);

    [RequireNamedArgs]
    Task<T?> GetViewModelAsync(int userId, int tenantId);
    Task<int> SaveAsync(T item);
}
