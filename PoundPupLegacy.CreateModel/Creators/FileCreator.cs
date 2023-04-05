namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class FileCreator : EntityCreator<File>
{
    private readonly IDatabaseInserterFactory<File> _fileInserterFactory;
    private readonly IDatabaseInserterFactory<TenantFile> _tenantFileInserterFactory;
    public FileCreator(
        IDatabaseInserterFactory<File> fileInserterFactory,
        IDatabaseInserterFactory<TenantFile> tenantFileInserterFactory
    )
    {
        _fileInserterFactory = fileInserterFactory;
        _tenantFileInserterFactory = tenantFileInserterFactory;
    }
    public override async Task CreateAsync(IAsyncEnumerable<File> files, IDbConnection connection)
    {

        await using var fileWriter = await _fileInserterFactory.CreateAsync(connection);
        await using var tenantFileWriter = await _tenantFileInserterFactory.CreateAsync(connection);

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
