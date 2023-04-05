namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class EditOwnNodeActionCreator : EntityCreator<EditOwnNodeAction>
{
    private readonly IDatabaseInserterFactory<Action> _actionInserterFactory;
    private readonly IDatabaseInserterFactory<EditOwnNodeAction> _editOwnNodeActionInserterFactory;
    public EditOwnNodeActionCreator(
        IDatabaseInserterFactory<Action> actionInserterFactory,
        IDatabaseInserterFactory<EditOwnNodeAction> editOwnNodeActionInserterFactory
    )
    {
        _actionInserterFactory = actionInserterFactory;
        _editOwnNodeActionInserterFactory = editOwnNodeActionInserterFactory;
    }
    public override async Task CreateAsync(IAsyncEnumerable<EditOwnNodeAction> actions, IDbConnection connection)
    {

        await using var actionWriter = await _actionInserterFactory.CreateAsync(connection);
        await using var editOwnNodeActionWriter = await _editOwnNodeActionInserterFactory.CreateAsync(connection);

        await foreach (var action in actions) {
            await actionWriter.InsertAsync(action);
            await editOwnNodeActionWriter.InsertAsync(action);
        }
    }
}
