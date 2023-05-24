namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class SenatorSenateBillActionCreatorFactory(
    IDatabaseInserterFactory<EventuallyIdentifiableNode> nodeInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableSenatorSenateBillAction> senatorSenateBillActionInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory
) : INodeCreatorFactory<EventuallyIdentifiableSenatorSenateBillAction>
{
    public async Task<NodeCreator<EventuallyIdentifiableSenatorSenateBillAction>> CreateAsync(IDbConnection connection) => 
        new(
            new() {
                await nodeInserterFactory.CreateAsync(connection),
                await senatorSenateBillActionInserterFactory.CreateAsync(connection)
            },
            await nodeDetailsCreatorFactory.CreateAsync(connection)
       );
}
