namespace PoundPupLegacy.DomainModel.Creators;

internal sealed class BasicActionCreatorFactory(
    IDatabaseInserterFactory<Action> actionInserterFactory,
    IDatabaseInserterFactory<BasicAction> basicActionInserterFactory
) : IEntityCreatorFactory<BasicAction>
{
    public async Task<IEntityCreator<BasicAction>> CreateAsync(IDbConnection connection) =>
        new InsertingEntityCreator<BasicAction>(new() {
            await actionInserterFactory.CreateAsync(connection),
            await basicActionInserterFactory.CreateAsync(connection)
        });
}
