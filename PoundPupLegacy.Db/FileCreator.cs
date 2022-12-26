using Npgsql;
using PoundPupLegacy.Db.Writers;
namespace PoundPupLegacy.Db;

public class FileCreator : IEntityCreator<Model.File>
{
    public static void Create(IEnumerable<Model.File> files, NpgsqlConnection connection)
    {

        using var fileWriter = FileWriter.Create(connection);

        foreach (var file in files)
        {
            fileWriter.Write(file);
        }
    }
}
