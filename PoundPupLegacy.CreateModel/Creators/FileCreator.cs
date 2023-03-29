namespace PoundPupLegacy.CreateModel.Creators;

public class FileCreator : IEntityCreator<File>
{
    public static async Task CreateAsync(IAsyncEnumerable<File> files, NpgsqlConnection connection)
    {

        await using var fileWriter = await FileInserter.CreateAsync(connection);
        await using var tenantFileWriter = await TenantFileInserter.CreateAsync(connection);

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
