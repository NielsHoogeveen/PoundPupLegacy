namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class CasePartyTypeCreatorFactory(
    IDatabaseInserterFactory<NodeToCreate> nodeInserterFactory,
    IDatabaseInserterFactory<SearchableToCreate> searchableInserterFactory,
    IDatabaseInserterFactory<NameableToCreate> nameableInserterFactory,
    IDatabaseInserterFactory<CasePartyType.ToCreate> casePartyTypeInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory,
    TermCreatorFactory nameableDetailsCreatorFactory
) : IEntityCreatorFactory<CasePartyType.ToCreate>
{
    public async Task<IEntityCreator<CasePartyType.ToCreate>> CreateAsync(IDbConnection connection) =>
        new NameableCreator<CasePartyType.ToCreate>(
            new () {
                await nodeInserterFactory.CreateAsync(connection),
                await searchableInserterFactory.CreateAsync(connection),
                await nameableInserterFactory.CreateAsync(connection),
                await casePartyTypeInserterFactory.CreateAsync(connection)
            },
            await nodeDetailsCreatorFactory.CreateAsync(connection),
            await nameableDetailsCreatorFactory.CreateAsync(connection)
        );
}
