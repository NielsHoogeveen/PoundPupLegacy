namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class BasicNameableTypeCreatorFactory(
    IDatabaseInserterFactory<EventuallyIdentifiableNodeType> nodeTypeInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableNameableType> nameableTypeInserterFactory
) : IEntityCreatorFactory<BasicNameableType>
{
    public async Task<IEntityCreator<BasicNameableType>> CreateAsync(IDbConnection connection) =>
        new InsertingEntityCreator<BasicNameableType>(new () {
            await nodeTypeInserterFactory.CreateAsync(connection),
            await nameableTypeInserterFactory.CreateAsync(connection)
        });
}
