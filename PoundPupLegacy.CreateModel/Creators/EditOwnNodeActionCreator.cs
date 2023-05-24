namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class EditOwnNodeActionCreatorFactory(
    IDatabaseInserterFactory<Action> actionInserterFactory,
    IDatabaseInserterFactory<EditOwnNodeAction> editOwnNodeActionInserterFactory
) : IInsertingEntityCreatorFactory<EditOwnNodeAction>
{
    public async Task<InsertingEntityCreator<EditOwnNodeAction>> CreateAsync(IDbConnection connection) =>
        new(new(){
            await actionInserterFactory.CreateAsync(connection),
            await editOwnNodeActionInserterFactory.CreateAsync(connection)
        });
}
