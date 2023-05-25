namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class SenatorSenateBillActionCreatorFactory(
    IDatabaseInserterFactory<EventuallyIdentifiableNode> nodeInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableSenatorSenateBillAction> senatorSenateBillActionInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory
) : IEntityCreatorFactory<EventuallyIdentifiableSenatorSenateBillAction>
{
    public async Task<IEntityCreator<EventuallyIdentifiableSenatorSenateBillAction>> CreateAsync(IDbConnection connection) => 
        new NodeCreator<EventuallyIdentifiableSenatorSenateBillAction>(
            new() {
                await nodeInserterFactory.CreateAsync(connection),
                await senatorSenateBillActionInserterFactory.CreateAsync(connection)
            },
            await nodeDetailsCreatorFactory.CreateAsync(connection)
       );
}
