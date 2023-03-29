namespace PoundPupLegacy.CreateModel.Creators;

public class EditNodeActionCreator : IEntityCreator<EditNodeAction>
{
    public static async Task CreateAsync(IAsyncEnumerable<EditNodeAction> actions, NpgsqlConnection connection)
    {

        await using var actionWriter = await ActionInserter.CreateAsync(connection);
        await using var editNodeActionWriter = await EditNodeActionInserter.CreateAsync(connection);

        await foreach (var action in actions) {
            await actionWriter.InsertAsync(action);
            await editNodeActionWriter.InsertAsync(action);
        }
    }
}
