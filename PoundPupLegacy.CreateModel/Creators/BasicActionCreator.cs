namespace PoundPupLegacy.CreateModel.Creators;

public class BasicActionCreator : IEntityCreator<BasicAction>
{
    public async Task CreateAsync(IAsyncEnumerable<BasicAction> actions, IDbConnection connection)
    {

        await using var actionWriter = await ActionInserter.CreateAsync(connection);
        await using var basicActionWriter = await BasicActionInserter.CreateAsync(connection);

        await foreach (var action in actions) {
            await actionWriter.InsertAsync(action);
            await basicActionWriter.InsertAsync(action);
        }
    }
}
