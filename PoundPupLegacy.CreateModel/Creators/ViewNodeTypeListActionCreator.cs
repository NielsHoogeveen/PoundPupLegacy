namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class ViewNodeTypeListActionCreatorFactory(
    IDatabaseInserterFactory<ViewNodeTypeListAction> viewNodeTypeListActionInserterFactory
) : IEntityCreatorFactory<ViewNodeTypeListAction>
{
    public async Task<IEntityCreator<ViewNodeTypeListAction>> CreateAsync(IDbConnection connection) =>
        new InsertingEntityCreator<ViewNodeTypeListAction>(new()
        {
            await viewNodeTypeListActionInserterFactory.CreateAsync(connection)
        });
        
}
