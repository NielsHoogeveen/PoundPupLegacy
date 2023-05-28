namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class RepresentativeHouseBillActionCreatorFactory(
    IDatabaseInserterFactory<NodeToCreate> nodeInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableRepresentativeHouseBillAction> representativeHouseBillActionInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory
) : IEntityCreatorFactory<EventuallyIdentifiableRepresentativeHouseBillAction>
{
    public async Task<IEntityCreator<EventuallyIdentifiableRepresentativeHouseBillAction>> CreateAsync(IDbConnection connection) => 
        new NodeCreator<EventuallyIdentifiableRepresentativeHouseBillAction>(
            new() {
                await nodeInserterFactory.CreateAsync(connection),
                await representativeHouseBillActionInserterFactory.CreateAsync(connection)
            },
            await nodeDetailsCreatorFactory.CreateAsync(connection)
        );
}
