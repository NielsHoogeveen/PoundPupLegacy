namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class ViewNodeTypeListActionCreatorFactory(
    IDatabaseInserterFactory<ViewNodeTypeListAction> viewNodeTypeListActionInserterFactory
) : IInsertingEntityCreatorFactory<ViewNodeTypeListAction>
{
    public async Task<InsertingEntityCreator<ViewNodeTypeListAction>> CreateAsync(IDbConnection connection) =>
        new(new()
        {
            await viewNodeTypeListActionInserterFactory.CreateAsync(connection)
        });
        
}
