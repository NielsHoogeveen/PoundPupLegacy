namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class NodeTermCreatorFactory(
    IDatabaseInserterFactory<NodeTerm> nodeTermInserterFactory
) : IInsertingEntityCreatorFactory<NodeTerm>
{
    public async Task<InsertingEntityCreator<NodeTerm>> CreateAsync(IDbConnection connection) =>
        new(new() { 
                await nodeTermInserterFactory.CreateAsync(connection) 
        });
}
