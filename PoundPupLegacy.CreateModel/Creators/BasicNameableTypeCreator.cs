namespace PoundPupLegacy.DomainModel.Creators;

internal sealed class BasicNameableTypeCreatorFactory(
    IDatabaseInserterFactory<NodeTypeToAdd> nodeTypeInserterFactory,
    IDatabaseInserterFactory<NameableTypeToAdd> nameableTypeInserterFactory
) : IEntityCreatorFactory<BasicNameableType>
{
    public async Task<IEntityCreator<BasicNameableType>> CreateAsync(IDbConnection connection) =>
        new InsertingEntityCreator<BasicNameableType>(new() {
            await nodeTypeInserterFactory.CreateAsync(connection),
            await nameableTypeInserterFactory.CreateAsync(connection)
        });
}
