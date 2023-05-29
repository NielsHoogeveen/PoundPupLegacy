namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class PartyPoliticalEntityRelationTypeCreatorFactory(
    IDatabaseInserterFactory<NodeToCreate> nodeInserterFactory,
    IDatabaseInserterFactory<SearchableToCreate> searchableInserterFactory,
    IDatabaseInserterFactory<NameableToCreate> nameableInserterFactory,
    IDatabaseInserterFactory<PartyPoliticalEntityRelationType.PartyPoliticalEntityRelationTypeToCreate> politicalEntityRelationTypeInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory,
    TermCreatorFactory nameableDetailsCreatorFactory
) : IEntityCreatorFactory<PartyPoliticalEntityRelationType.PartyPoliticalEntityRelationTypeToCreate>
{
    public async Task<IEntityCreator<PartyPoliticalEntityRelationType.PartyPoliticalEntityRelationTypeToCreate>> CreateAsync(IDbConnection connection) =>
        new NameableCreator<PartyPoliticalEntityRelationType.PartyPoliticalEntityRelationTypeToCreate>(
            new() {
                await nodeInserterFactory.CreateAsync(connection),
                await searchableInserterFactory.CreateAsync(connection),
                await nameableInserterFactory.CreateAsync(connection),
                await politicalEntityRelationTypeInserterFactory.CreateAsync(connection)
            },
            await nodeDetailsCreatorFactory.CreateAsync(connection),
            await nameableDetailsCreatorFactory.CreateAsync(connection)
        );
}
