namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class NodeFileCreatorFactory(
    IDatabaseInserterFactory<NodeFile> nodeFileInserterFactory
) : IInsertingEntityCreatorFactory<NodeFile>
{
    public async Task<InsertingEntityCreator<NodeFile>> CreateAsync(IDbConnection connection) =>
        new(new() {
            await nodeFileInserterFactory.CreateAsync(connection)
        });
}
