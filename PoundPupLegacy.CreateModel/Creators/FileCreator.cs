namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class FileCreatorFactory(
    IDatabaseInserterFactory<File> fileInserterFactory,
    IDatabaseInserterFactory<TenantFile> tenantFileInserterFactory
) : IEntityCreatorFactory<File>
{
    public async Task<IEntityCreator<File>> CreateAsync(IDbConnection connection) =>
    new FileCreator(
        new() {
            await fileInserterFactory.CreateAsync(connection)
        },
        await tenantFileInserterFactory.CreateAsync(connection)
    );
}

public class FileCreator(
    List<IDatabaseInserter<File>> inserters,
    IDatabaseInserter<TenantFile> tenantFileInserter
) : InsertingEntityCreator<File>(inserters)
{
    public override async Task ProcessAsync(File element)
    {
        await base.ProcessAsync(element);
        foreach (var tenantFile in element.TenantFiles) {
            tenantFile.FileId = element.Id;
            tenantFile.TenantFileId ??= element.Id;
            await tenantFileInserter.InsertAsync(tenantFile);
        }
    }
    public override async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();
        await tenantFileInserter.DisposeAsync();
    }
}