namespace PoundPupLegacy.DomainModel.Creators;

internal sealed class InterCountryRelationCreatorFactory(
    IDatabaseInserterFactory<NodeToCreate> nodeInserterFactory,
    IDatabaseInserterFactory<InterCountryRelation.ToCreate> interCountryRelationInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory
) : IEntityCreatorFactory<InterCountryRelation.ToCreate>
{
    public async Task<IEntityCreator<InterCountryRelation.ToCreate>> CreateAsync(IDbConnection connection) =>
        new NodeCreator<InterCountryRelation.ToCreate>(
            new()
            {
                await nodeInserterFactory.CreateAsync(connection),
                await interCountryRelationInserterFactory.CreateAsync(connection)
            },
            await nodeDetailsCreatorFactory.CreateAsync(connection)
        );
}
