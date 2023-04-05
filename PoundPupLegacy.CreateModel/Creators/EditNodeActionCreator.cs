namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class EditNodeActionCreator : EntityCreator<EditNodeAction>
{
    private readonly IDatabaseInserterFactory<Action> _actionInserterFactory;
    private readonly IDatabaseInserterFactory<EditNodeAction> _editNodeActionInserterFactory;
    public EditNodeActionCreator(
        IDatabaseInserterFactory<Action> actionInserterFactory,
        IDatabaseInserterFactory<EditNodeAction> editNodeActionInserterFactory
    )
    {
        _actionInserterFactory = actionInserterFactory;
        _editNodeActionInserterFactory = editNodeActionInserterFactory;
    }
    public override async Task CreateAsync(IAsyncEnumerable<EditNodeAction> actions, IDbConnection connection)
    {

        await using var actionWriter = await _actionInserterFactory.CreateAsync(connection);
        await using var editNodeActionWriter = await _editNodeActionInserterFactory.CreateAsync(connection);

        await foreach (var action in actions) {
            await actionWriter.InsertAsync(action);
            await editNodeActionWriter.InsertAsync(action);
        }
    }
}
