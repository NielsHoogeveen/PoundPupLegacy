namespace PoundPupLegacy.Db;

public class DeleteNodeActionCreator : IEntityCreator<DeleteNodeAction>
{
    public static async Task CreateAsync(IAsyncEnumerable<DeleteNodeAction> actions, NpgsqlConnection connection)
    {

        await using var actionWriter = await ActionWriter.CreateAsync(connection);
        await using var deleteNodeActionWriter = await DeleteNodeActionWriter.CreateAsync(connection);

        await foreach (var action in actions) {
            await actionWriter.WriteAsync(action);
            await deleteNodeActionWriter.WriteAsync(action);
        }
    }
}
