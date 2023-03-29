namespace PoundPupLegacy.CreateModel.Creators;

public class EditOwnNodeActionCreator : IEntityCreator<EditOwnNodeAction>
{
    public static async Task CreateAsync(IAsyncEnumerable<EditOwnNodeAction> actions, NpgsqlConnection connection)
    {

        await using var actionWriter = await ActionWriter.CreateAsync(connection);
        await using var editOwnNodeActionWriter = await EditOwnNodeActionWriter.CreateAsync(connection);

        await foreach (var action in actions) {
            await actionWriter.WriteAsync(action);
            await editOwnNodeActionWriter.WriteAsync(action);
        }
    }
}
