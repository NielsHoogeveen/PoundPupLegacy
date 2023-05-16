namespace PoundPupLegacy.EditModel.UI.Services;

public interface ISaveService<T>
{
    Task SaveAsync(T item, IDbConnection connection);
}
