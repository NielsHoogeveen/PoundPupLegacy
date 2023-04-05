namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class CreateNodeActionCreator : EntityCreator<CreateNodeAction>
{
    private readonly IDatabaseInserterFactory<Action> _actionInserterFactory;
    private readonly IDatabaseInserterFactory<CreateNodeAction> _createNodeActionInserterFactory;
    public CreateNodeActionCreator(
               IDatabaseInserterFactory<Action> actionInserterFactory,
                      IDatabaseInserterFactory<CreateNodeAction> createNodeActionInserterFactory
           )
    {
        _actionInserterFactory = actionInserterFactory;
        _createNodeActionInserterFactory = createNodeActionInserterFactory;
    }


    public override async Task CreateAsync(IAsyncEnumerable<CreateNodeAction> actions, IDbConnection connection)
    {

        await using var actionWriter = await _actionInserterFactory.CreateAsync(connection);
        await using var createNodeActionWriter = await _createNodeActionInserterFactory.CreateAsync(connection);

        await foreach (var action in actions) {
            await actionWriter.InsertAsync(action);
            await createNodeActionWriter.InsertAsync(action);
        }
    }
}
