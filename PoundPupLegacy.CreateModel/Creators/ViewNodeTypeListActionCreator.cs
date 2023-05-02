namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class ViewNodeTypeListActionCreator : EntityCreator<ViewNodeTypeListAction>
{
    private readonly IDatabaseInserterFactory<ViewNodeTypeListAction> _viewNodeTypeListActionInserterFactory;
    public ViewNodeTypeListActionCreator(
        IDatabaseInserterFactory<ViewNodeTypeListAction> viewNodeTypeListActionInserterFactory
        )
    {
        _viewNodeTypeListActionInserterFactory = viewNodeTypeListActionInserterFactory;
    }
    public override async Task CreateAsync(IAsyncEnumerable<ViewNodeTypeListAction> actions, IDbConnection connection)
    {

        await using var viewNodeTypeListActionWriter = await _viewNodeTypeListActionInserterFactory.CreateAsync(connection);

        await foreach (var action in actions) {
            await viewNodeTypeListActionWriter.InsertAsync(action);
        }
    }
}
