namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class RepresentativeHouseBillActionCreatorFactory(
    IDatabaseInserterFactory<EventuallyIdentifiableNode> nodeInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableRepresentativeHouseBillAction> representativeHouseBillActionInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory
) : INodeCreatorFactory<EventuallyIdentifiableRepresentativeHouseBillAction>
{
    public async Task<NodeCreator<EventuallyIdentifiableRepresentativeHouseBillAction>> CreateAsync(IDbConnection connection) => 
        new(
            new() {
                await nodeInserterFactory.CreateAsync(connection),
                await representativeHouseBillActionInserterFactory.CreateAsync(connection)
            },
            await nodeDetailsCreatorFactory.CreateAsync(connection)
        );
}
