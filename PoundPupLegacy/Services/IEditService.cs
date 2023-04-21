using PoundPupLegacy.Common;

namespace PoundPupLegacy.Services;

public interface IEditService<T>
{
    [RequireNamedArgs]
    Task<T?> GetViewModelAsync(int urlId, int userId, int tenantId);

    [RequireNamedArgs]
    Task<T?> GetViewModelAsync(int userId, int tenantId);
    Task SaveAsync(T item);
}
