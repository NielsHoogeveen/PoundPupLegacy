namespace PoundPupLegacy.DomainModel.Creators;

internal sealed class EditNodeActionCreatorFactory(
    IDatabaseInserterFactory<Action> actionInserterFactory,
    IDatabaseInserterFactory<EditNodeAction> editNodeActionInserterFactory
) : IEntityCreatorFactory<EditNodeAction>
{
    public async Task<IEntityCreator<EditNodeAction>> CreateAsync(IDbConnection connection) =>
        new InsertingEntityCreator<EditNodeAction>(new(){
            await actionInserterFactory.CreateAsync(connection),
            await editNodeActionInserterFactory.CreateAsync(connection)
        });
}
