namespace PoundPupLegacy.Db;

public class FileCreator : IEntityCreator<Model.File>
{
    public static async Task CreateAsync(IEnumerable<Model.File> files, NpgsqlConnection connection)
    {

        await using var fileWriter = await FileWriter.CreateAsync(connection);

        foreach (var file in files)
        {
            await fileWriter.WriteAsync(file);
        }
    }
}
