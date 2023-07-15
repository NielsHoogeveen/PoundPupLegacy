namespace PoundPupLegacy.DomainModel.Creators;

internal sealed class SenatorSenateBillActionCreatorFactory(
    IDatabaseInserterFactory<NodeToCreate> nodeInserterFactory,
    IDatabaseInserterFactory<SenatorSenateBillAction> senatorSenateBillActionInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory
) : IEntityCreatorFactory<SenatorSenateBillAction>
{
    public async Task<IEntityCreator<SenatorSenateBillAction>> CreateAsync(IDbConnection connection) =>
        new NodeCreator<SenatorSenateBillAction>(
            new() {
                await nodeInserterFactory.CreateAsync(connection),
                await senatorSenateBillActionInserterFactory.CreateAsync(connection)
            },
            await nodeDetailsCreatorFactory.CreateAsync(connection)
       );
}
