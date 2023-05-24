namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class DeleteNodeActionCreatorFactory(
    IDatabaseInserterFactory<Action> actionInserterFactory,
    IDatabaseInserterFactory<DeleteNodeAction> deleteNodeActionInserterFactory
) : IInsertingEntityCreatorFactory<DeleteNodeAction>
{
    public async Task<InsertingEntityCreator<DeleteNodeAction>> CreateAsync(IDbConnection connection) => 
        new (new () {
            await actionInserterFactory.CreateAsync(connection),
            await deleteNodeActionInserterFactory.CreateAsync(connection)
        });
}
