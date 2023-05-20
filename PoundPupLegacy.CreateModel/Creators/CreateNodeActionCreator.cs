namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class CreateNodeActionCreator(
    IDatabaseInserterFactory<Action> actionInserterFactory,
    IDatabaseInserterFactory<CreateNodeAction> createNodeActionInserterFactory
) : EntityCreator<CreateNodeAction>
{
    public override async Task CreateAsync(IAsyncEnumerable<CreateNodeAction> actions, IDbConnection connection)
    {
        await using var actionWriter = await actionInserterFactory.CreateAsync(connection);
        await using var createNodeActionWriter = await createNodeActionInserterFactory.CreateAsync(connection);

        await foreach (var action in actions) {
            await actionWriter.InsertAsync(action);
            await createNodeActionWriter.InsertAsync(action);
        }
    }
}
