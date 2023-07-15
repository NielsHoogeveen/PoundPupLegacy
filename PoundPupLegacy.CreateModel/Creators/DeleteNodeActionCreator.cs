namespace PoundPupLegacy.DomainModel.Creators;

internal sealed class DeleteNodeActionCreatorFactory(
    IDatabaseInserterFactory<Action> actionInserterFactory,
    IDatabaseInserterFactory<DeleteNodeAction> deleteNodeActionInserterFactory
) : IEntityCreatorFactory<DeleteNodeAction>
{
    public async Task<IEntityCreator<DeleteNodeAction>> CreateAsync(IDbConnection connection) =>
        new InsertingEntityCreator<DeleteNodeAction>(new() {
            await actionInserterFactory.CreateAsync(connection),
            await deleteNodeActionInserterFactory.CreateAsync(connection)
        });
}
