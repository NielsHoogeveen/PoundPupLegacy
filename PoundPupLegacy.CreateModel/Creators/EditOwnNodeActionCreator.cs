namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class EditOwnNodeActionCreator(
    IDatabaseInserterFactory<Action> actionInserterFactory,
    IDatabaseInserterFactory<EditOwnNodeAction> editOwnNodeActionInserterFactory
) : EntityCreator<EditOwnNodeAction>
{
    public override async Task CreateAsync(IAsyncEnumerable<EditOwnNodeAction> actions, IDbConnection connection)
    {
        await using var actionWriter = await actionInserterFactory.CreateAsync(connection);
        await using var editOwnNodeActionWriter = await editOwnNodeActionInserterFactory.CreateAsync(connection);

        await foreach (var action in actions) {
            await actionWriter.InsertAsync(action);
            await editOwnNodeActionWriter.InsertAsync(action);
        }
    }
}
