namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class CreateNodeActionCreatorFactory(
    IDatabaseInserterFactory<Action> actionInserterFactory,
    IDatabaseInserterFactory<CreateNodeAction> createNodeActionInserterFactory
) : IEntityCreatorFactory<CreateNodeAction>
{
    public async Task<IEntityCreator<CreateNodeAction>> CreateAsync(IDbConnection connection) => 
        new InsertingEntityCreator<CreateNodeAction>(new () {
            await actionInserterFactory.CreateAsync(connection),
            await createNodeActionInserterFactory.CreateAsync(connection)
        });
}
