namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class NodeTermCreatorFactory(
    IDatabaseInserterFactory<ResolvedNodeTermToAdd> nodeTermInserterFactory
) : IEntityCreatorFactory<ResolvedNodeTermToAdd>
{
    public async Task<IEntityCreator<ResolvedNodeTermToAdd>> CreateAsync(IDbConnection connection) =>
        new InsertingEntityCreator<ResolvedNodeTermToAdd>(new() { 
                await nodeTermInserterFactory.CreateAsync(connection) 
        });
}
