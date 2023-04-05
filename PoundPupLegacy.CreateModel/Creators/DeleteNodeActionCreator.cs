namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class DeleteNodeActionCreator : EntityCreator<DeleteNodeAction>
{
    private readonly IDatabaseInserterFactory<Action> _actionInserterFactory;
    private readonly IDatabaseInserterFactory<DeleteNodeAction> _deleteNodeActionInserterFactory;
    public DeleteNodeActionCreator(
        IDatabaseInserterFactory<Action> actionInserterFactory,
        IDatabaseInserterFactory<DeleteNodeAction> deleteNodeActionInserterFactory
    )
    {
        _actionInserterFactory = actionInserterFactory;
        _deleteNodeActionInserterFactory = deleteNodeActionInserterFactory;
    }
    public override async Task CreateAsync(IAsyncEnumerable<DeleteNodeAction> actions, IDbConnection connection)
    {

        await using var actionWriter = await _actionInserterFactory.CreateAsync(connection);
        await using var deleteNodeActionWriter = await _deleteNodeActionInserterFactory.CreateAsync(connection);

        await foreach (var action in actions) {
            await actionWriter.InsertAsync(action);
            await deleteNodeActionWriter.InsertAsync(action);
        }
    }
}
