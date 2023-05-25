namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class EditOwnNodeActionCreatorFactory(
    IDatabaseInserterFactory<Action> actionInserterFactory,
    IDatabaseInserterFactory<EditOwnNodeAction> editOwnNodeActionInserterFactory
) : IEntityCreatorFactory<EditOwnNodeAction>
{
    public async Task<IEntityCreator<EditOwnNodeAction>> CreateAsync(IDbConnection connection) =>
        new InsertingEntityCreator<EditOwnNodeAction>(new(){
            await actionInserterFactory.CreateAsync(connection),
            await editOwnNodeActionInserterFactory.CreateAsync(connection)
        });
}
