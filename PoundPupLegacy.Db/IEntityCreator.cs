using Npgsql;

namespace PoundPupLegacy.Db;

internal interface IEntityCreator<T>
{
    public abstract static void Create(IEnumerable<T> elements, NpgsqlConnection connection);
}
