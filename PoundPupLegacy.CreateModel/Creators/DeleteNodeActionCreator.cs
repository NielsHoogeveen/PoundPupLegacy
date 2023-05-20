namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class DeleteNodeActionCreator(
    IDatabaseInserterFactory<Action> actionInserterFactory,
    IDatabaseInserterFactory<DeleteNodeAction> deleteNodeActionInserterFactory
) : EntityCreator<DeleteNodeAction>
{
    public override async Task CreateAsync(IAsyncEnumerable<DeleteNodeAction> actions, IDbConnection connection)
    {
        await using var actionWriter = await actionInserterFactory.CreateAsync(connection);
        await using var deleteNodeActionWriter = await deleteNodeActionInserterFactory.CreateAsync(connection);

        await foreach (var action in actions) {
            await actionWriter.InsertAsync(action);
            await deleteNodeActionWriter.InsertAsync(action);
        }
    }
}
