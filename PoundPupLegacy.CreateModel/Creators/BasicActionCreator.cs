namespace PoundPupLegacy.CreateModel.Creators;

public class BasicActionCreator : IEntityCreator<BasicAction>
{
    public static async Task CreateAsync(IAsyncEnumerable<BasicAction> actions, NpgsqlConnection connection)
    {

        await using var actionWriter = await ActionInserter.CreateAsync(connection);
        await using var basicActionWriter = await BasicActionInserter.CreateAsync(connection);

        await foreach (var action in actions) {
            await actionWriter.WriteAsync(action);
            await basicActionWriter.WriteAsync(action);
        }
    }
}
