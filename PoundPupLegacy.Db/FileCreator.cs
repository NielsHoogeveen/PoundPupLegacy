namespace PoundPupLegacy.Db;

public class FileCreator : IEntityCreator<Model.File>
{
    public static async Task CreateAsync(IAsyncEnumerable<Model.File> files, NpgsqlConnection connection)
    {

        await using var fileWriter = await FileWriter.CreateAsync(connection);

        await foreach (var file in files)
        {
            await fileWriter.WriteAsync(file);
        }
    }
}
