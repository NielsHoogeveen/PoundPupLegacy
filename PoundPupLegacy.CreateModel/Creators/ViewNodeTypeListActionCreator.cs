namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class ViewNodeTypeListActionCreator(IDatabaseInserterFactory<ViewNodeTypeListAction> viewNodeTypeListActionInserterFactory) : EntityCreator<ViewNodeTypeListAction>
{
    public override async Task CreateAsync(IAsyncEnumerable<ViewNodeTypeListAction> actions, IDbConnection connection)
    {
        await using var viewNodeTypeListActionWriter = await viewNodeTypeListActionInserterFactory.CreateAsync(connection);

        await foreach (var action in actions) {
            await viewNodeTypeListActionWriter.InsertAsync(action);
        }
    }
}
