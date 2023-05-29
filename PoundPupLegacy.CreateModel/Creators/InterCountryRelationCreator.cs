namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class InterCountryRelationCreatorFactory(
    IDatabaseInserterFactory<NodeToCreate> nodeInserterFactory,
    IDatabaseInserterFactory<InterCountryRelation.InterCountryRelationToCreate> interCountryRelationInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory
) : IEntityCreatorFactory<InterCountryRelation.InterCountryRelationToCreate>
{
    public async Task<IEntityCreator<InterCountryRelation.InterCountryRelationToCreate>> CreateAsync(IDbConnection connection) =>
        new NodeCreator<InterCountryRelation.InterCountryRelationToCreate>(
            new () 
            {
                await nodeInserterFactory.CreateAsync(connection),
                await interCountryRelationInserterFactory.CreateAsync(connection)
            },
            await nodeDetailsCreatorFactory.CreateAsync(connection)
        );
}
