namespace PoundPupLegacy.CreateModel.Creators;

public class EditOwnNodeActionCreator : IEntityCreator<EditOwnNodeAction>
{
    public async Task CreateAsync(IAsyncEnumerable<EditOwnNodeAction> actions, IDbConnection connection)
    {

        await using var actionWriter = await ActionInserter.CreateAsync(connection);
        await using var editOwnNodeActionWriter = await EditOwnNodeActionInserter.CreateAsync(connection);

        await foreach (var action in actions) {
            await actionWriter.InsertAsync(action);
            await editOwnNodeActionWriter.InsertAsync(action);
        }
    }
}
