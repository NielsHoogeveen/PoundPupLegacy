namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class BasicActionCreatorFactory(
    IDatabaseInserterFactory<Action> actionInserterFactory,
    IDatabaseInserterFactory<BasicAction> basicActionInserterFactory
) : IInsertingEntityCreatorFactory<BasicAction>
{
    public async Task<InsertingEntityCreator<BasicAction>> CreateAsync(IDbConnection connection) =>
        new (new () {
            await actionInserterFactory.CreateAsync(connection),
            await basicActionInserterFactory.CreateAsync(connection)
        });
}
