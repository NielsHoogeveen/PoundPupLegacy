namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class NodeFileCreatorFactory(
    IDatabaseInserterFactory<NodeFile> nodeFileInserterFactory
) : IEntityCreatorFactory<NodeFile>
{
    public async Task<IEntityCreator<NodeFile>> CreateAsync(IDbConnection connection) =>
        new InsertingEntityCreator<NodeFile>(new() {
            await nodeFileInserterFactory.CreateAsync(connection)
        });
}
