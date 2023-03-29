namespace PoundPupLegacy.CreateModel.Creators;

public class EditNodeActionCreator : IEntityCreator<EditNodeAction>
{
    public static async Task CreateAsync(IAsyncEnumerable<EditNodeAction> actions, NpgsqlConnection connection)
    {

        await using var actionWriter = await ActionWriter.CreateAsync(connection);
        await using var editNodeActionWriter = await EditNodeActionWriter.CreateAsync(connection);

        await foreach (var action in actions) {
            await actionWriter.WriteAsync(action);
            await editNodeActionWriter.WriteAsync(action);
        }
    }
}
