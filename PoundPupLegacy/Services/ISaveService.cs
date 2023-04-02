using System.Data;

namespace PoundPupLegacy.Services;

public interface ISaveService<T>
{
    Task Save(T item, IDbConnection connection);
}
