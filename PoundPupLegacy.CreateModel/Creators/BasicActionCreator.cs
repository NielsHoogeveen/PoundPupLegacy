namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class BasicActionCreator : EntityCreator<BasicAction>
{
    private readonly IDatabaseInserterFactory<Action> _actionInserterFactory;
    private readonly IDatabaseInserterFactory<BasicAction> _basicActionInserterFactory;
    public BasicActionCreator(
        IDatabaseInserterFactory<Action> actionInserterFactory,
        IDatabaseInserterFactory<BasicAction> basicActionInserterFactory
        )
    {
        _actionInserterFactory = actionInserterFactory;
        _basicActionInserterFactory = basicActionInserterFactory;

    }
    public override async Task CreateAsync(IAsyncEnumerable<BasicAction> actions, IDbConnection connection)
    {

        await using var actionWriter = await _actionInserterFactory.CreateAsync(connection);
        await using var basicActionWriter = await _basicActionInserterFactory.CreateAsync(connection);

        await foreach (var action in actions) {
            await actionWriter.InsertAsync(action);
            await basicActionWriter.InsertAsync(action);
        }
    }
}
