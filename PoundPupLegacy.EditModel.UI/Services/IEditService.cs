namespace PoundPupLegacy.EditModel.UI.Services;

public interface IEditService<TRead, TSave>
    where TSave: TRead
{
    [RequireNamedArgs]
    Task<TRead?> GetViewModelAsync(int urlId, int userId, int tenantId);

    [RequireNamedArgs]
    Task<TRead?> GetViewModelAsync(int userId, int tenantId);
    Task<int> SaveAsync(TSave item);
}
