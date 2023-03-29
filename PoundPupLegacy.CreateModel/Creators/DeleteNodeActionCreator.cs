namespace PoundPupLegacy.CreateModel.Creators;

public class DeleteNodeActionCreator : IEntityCreator<DeleteNodeAction>
{
    public static async Task CreateAsync(IAsyncEnumerable<DeleteNodeAction> actions, NpgsqlConnection connection)
    {

        await using var actionWriter = await ActionInserter.CreateAsync(connection);
        await using var deleteNodeActionWriter = await DeleteNodeActionInserter.CreateAsync(connection);

        await foreach (var action in actions) {
            await actionWriter.InsertAsync(action);
            await deleteNodeActionWriter.InsertAsync(action);
        }
    }
}
