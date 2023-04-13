namespace PoundPupLegacy.Services;

public interface IEditService<T>
{
    Task<T?> GetViewModelAsync(int urlId, int userId, int tenantId);
    Task<T?> GetViewModelAsync(int userId, int tenantId);
    Task SaveAsync(T item);
}
