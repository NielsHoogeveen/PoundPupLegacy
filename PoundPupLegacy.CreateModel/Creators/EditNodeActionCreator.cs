namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class EditNodeActionCreator(
    IDatabaseInserterFactory<Action> actionInserterFactory,
    IDatabaseInserterFactory<EditNodeAction> editNodeActionInserterFactory
) : EntityCreator<EditNodeAction>
{
    public override async Task CreateAsync(IAsyncEnumerable<EditNodeAction> actions, IDbConnection connection)
    {
        await using var actionWriter = await actionInserterFactory.CreateAsync(connection);
        await using var editNodeActionWriter = await editNodeActionInserterFactory.CreateAsync(connection);

        await foreach (var action in actions) {
            await actionWriter.InsertAsync(action);
            await editNodeActionWriter.InsertAsync(action);
        }
    }
}
