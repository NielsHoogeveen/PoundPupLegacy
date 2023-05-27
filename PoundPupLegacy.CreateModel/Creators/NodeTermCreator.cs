namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class NodeTermCreatorFactory(
    IDatabaseInserterFactory<NodeTermToAdd> nodeTermInserterFactory
) : IEntityCreatorFactory<NodeTermToAdd>
{
    public async Task<IEntityCreator<NodeTermToAdd>> CreateAsync(IDbConnection connection) =>
        new InsertingEntityCreator<NodeTermToAdd>(new() { 
                await nodeTermInserterFactory.CreateAsync(connection) 
        });
}
