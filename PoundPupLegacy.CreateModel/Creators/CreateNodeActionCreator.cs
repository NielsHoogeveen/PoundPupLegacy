namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class CreateNodeActionCreatorFactory(
    IDatabaseInserterFactory<Action> actionInserterFactory,
    IDatabaseInserterFactory<CreateNodeAction> createNodeActionInserterFactory
) : IInsertingEntityCreatorFactory<CreateNodeAction>
{
    public async Task<InsertingEntityCreator<CreateNodeAction>> CreateAsync(IDbConnection connection) => 
        new InsertingEntityCreator<CreateNodeAction>(new () {
            await actionInserterFactory.CreateAsync(connection),
            await createNodeActionInserterFactory.CreateAsync(connection)
        });
}
