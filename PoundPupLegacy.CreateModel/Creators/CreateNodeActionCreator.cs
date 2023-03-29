namespace PoundPupLegacy.CreateModel.Creators;

public class CreateNodeActionCreator : IEntityCreator<CreateNodeAction>
{
    public static async Task CreateAsync(IAsyncEnumerable<CreateNodeAction> actions, NpgsqlConnection connection)
    {

        await using var actionWriter = await ActionInserter.CreateAsync(connection);
        await using var createNodeActionWriter = await CreateNodeActionInserter.CreateAsync(connection);

        await foreach (var action in actions) {
            await actionWriter.InsertAsync(action);
            await createNodeActionWriter.InsertAsync(action);
        }
    }
}
