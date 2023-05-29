namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class RepresentativeHouseBillActionCreatorFactory(
    IDatabaseInserterFactory<NodeToCreate> nodeInserterFactory,
    IDatabaseInserterFactory<RepresentativeHouseBillAction> representativeHouseBillActionInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory
) : IEntityCreatorFactory<RepresentativeHouseBillAction>
{
    public async Task<IEntityCreator<RepresentativeHouseBillAction>> CreateAsync(IDbConnection connection) => 
        new NodeCreator<RepresentativeHouseBillAction>(
            new() {
                await nodeInserterFactory.CreateAsync(connection),
                await representativeHouseBillActionInserterFactory.CreateAsync(connection)
            },
            await nodeDetailsCreatorFactory.CreateAsync(connection)
        );
}
