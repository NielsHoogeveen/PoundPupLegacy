namespace PoundPupLegacy.CreateModel.Creators;

public class CreateNodeActionCreator : IEntityCreator<CreateNodeAction>
{
    public static async Task CreateAsync(IAsyncEnumerable<CreateNodeAction> actions, NpgsqlConnection connection)
    {

        await using var actionWriter = await ActionWriter.CreateAsync(connection);
        await using var createNodeActionWriter = await CreateNodeActionWriter.CreateAsync(connection);

        await foreach (var action in actions) {
            await actionWriter.WriteAsync(action);
            await createNodeActionWriter.WriteAsync(action);
        }
    }
}
