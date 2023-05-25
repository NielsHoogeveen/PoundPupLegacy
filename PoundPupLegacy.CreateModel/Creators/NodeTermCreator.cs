namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class NodeTermCreatorFactory(
    IDatabaseInserterFactory<NodeTerm> nodeTermInserterFactory
) : IEntityCreatorFactory<NodeTerm>
{
    public async Task<IEntityCreator<NodeTerm>> CreateAsync(IDbConnection connection) =>
        new InsertingEntityCreator<NodeTerm>(new() { 
                await nodeTermInserterFactory.CreateAsync(connection) 
        });
}
