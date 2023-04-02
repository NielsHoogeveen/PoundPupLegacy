namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class CreateNodeActionCreator : IEntityCreator<CreateNodeAction>
{
    public async Task CreateAsync(IAsyncEnumerable<CreateNodeAction> actions, IDbConnection connection)
    {

        await using var actionWriter = await ActionInserter.CreateAsync(connection);
        await using var createNodeActionWriter = await CreateNodeActionInserter.CreateAsync(connection);

        await foreach (var action in actions) {
            await actionWriter.InsertAsync(action);
            await createNodeActionWriter.InsertAsync(action);
        }
    }
}
