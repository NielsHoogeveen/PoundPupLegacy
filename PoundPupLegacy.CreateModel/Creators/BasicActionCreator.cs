namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class BasicActionCreator(
    IDatabaseInserterFactory<Action> actionInserterFactory,
    IDatabaseInserterFactory<BasicAction> basicActionInserterFactory
) : EntityCreator<BasicAction>
{
    public override async Task CreateAsync(IAsyncEnumerable<BasicAction> actions, IDbConnection connection)
    {

        await using var actionWriter = await actionInserterFactory.CreateAsync(connection);
        await using var basicActionWriter = await basicActionInserterFactory.CreateAsync(connection);

        await foreach (var action in actions) {
            await actionWriter.InsertAsync(action);
            await basicActionWriter.InsertAsync(action);
        }
    }
}
