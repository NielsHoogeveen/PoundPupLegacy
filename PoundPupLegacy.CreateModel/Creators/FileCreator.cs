namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class FileCreator(
    IDatabaseInserterFactory<File> fileInserterFactory,
    IDatabaseInserterFactory<TenantFile> tenantFileInserterFactory
) : EntityCreator<File>
{
    public override async Task CreateAsync(IAsyncEnumerable<File> files, IDbConnection connection)
    {

        await using var fileWriter = await fileInserterFactory.CreateAsync(connection);
        await using var tenantFileWriter = await tenantFileInserterFactory.CreateAsync(connection);

        await foreach (var file in files) {
            await fileWriter.InsertAsync(file);
            foreach (var tenantFile in file.TenantFiles) {
                tenantFile.FileId = file.Id;
                tenantFile.TenantFileId ??= file.Id;
                await tenantFileWriter.InsertAsync(tenantFile);
            }

        }
    }
}
