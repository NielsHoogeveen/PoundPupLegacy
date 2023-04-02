namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class DeleteNodeActionCreator : IEntityCreator<DeleteNodeAction>
{
    public async Task CreateAsync(IAsyncEnumerable<DeleteNodeAction> actions, IDbConnection connection)
    {

        await using var actionWriter = await ActionInserter.CreateAsync(connection);
        await using var deleteNodeActionWriter = await DeleteNodeActionInserter.CreateAsync(connection);

        await foreach (var action in actions) {
            await actionWriter.InsertAsync(action);
            await deleteNodeActionWriter.InsertAsync(action);
        }
    }
}
