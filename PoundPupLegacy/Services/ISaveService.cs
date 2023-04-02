using System.Data;

namespace PoundPupLegacy.Services;

public interface ISaveService<T>
{
    Task SaveAsync(T item, IDbConnection connection);
}
