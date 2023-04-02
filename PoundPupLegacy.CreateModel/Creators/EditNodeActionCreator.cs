namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class EditNodeActionCreator : IEntityCreator<EditNodeAction>
{
    public async Task CreateAsync(IAsyncEnumerable<EditNodeAction> actions, IDbConnection connection)
    {

        await using var actionWriter = await ActionInserter.CreateAsync(connection);
        await using var editNodeActionWriter = await EditNodeActionInserter.CreateAsync(connection);

        await foreach (var action in actions) {
            await actionWriter.InsertAsync(action);
            await editNodeActionWriter.InsertAsync(action);
        }
    }
}
