namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class EditNodeActionCreatorFactory(
    IDatabaseInserterFactory<Action> actionInserterFactory,
    IDatabaseInserterFactory<EditNodeAction> editNodeActionInserterFactory
) : IInsertingEntityCreatorFactory<EditNodeAction>
{
    public async Task<InsertingEntityCreator<EditNodeAction>> CreateAsync(IDbConnection connection) =>
        new ( new (){
            await actionInserterFactory.CreateAsync(connection),
            await editNodeActionInserterFactory.CreateAsync(connection)
        });
}
