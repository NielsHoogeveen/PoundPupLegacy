namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class BasicNameableTypeCreatorFactory(
    IDatabaseInserterFactory<NodeType> nodeTypeInserterFactory,
    IDatabaseInserterFactory<NameableType> nameableTypeInserterFactory
) : IInsertingEntityCreatorFactory<BasicNameableType>
{
    public async Task<InsertingEntityCreator<BasicNameableType>> CreateAsync(IDbConnection connection) =>
        new (new () {
            await nodeTypeInserterFactory.CreateAsync(connection),
            await nameableTypeInserterFactory.CreateAsync(connection)
        });
}
