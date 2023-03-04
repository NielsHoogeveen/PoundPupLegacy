namespace PoundPupLegacy.Db;

public class FileCreator : IEntityCreator<Model.File>
{
    public static async Task CreateAsync(IAsyncEnumerable<Model.File> files, NpgsqlConnection connection)
    {

        await using var fileWriter = await FileWriter.CreateAsync(connection);
        await using var tenantFileWriter = await TenantFileWriter.CreateAsync(connection);

        await foreach (var file in files) {
            await fileWriter.WriteAsync(file);
            foreach (var tenantFile in file.TenantFiles) {
                tenantFile.FileId = file.Id;
                tenantFile.TenantFileId ??= file.Id;
                await tenantFileWriter.WriteAsync(tenantFile);
            }

        }
    }
}
