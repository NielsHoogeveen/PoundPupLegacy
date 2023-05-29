namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class CasePartyTypeCreatorFactory(
    IDatabaseInserterFactory<NodeToCreate> nodeInserterFactory,
    IDatabaseInserterFactory<SearchableToCreate> searchableInserterFactory,
    IDatabaseInserterFactory<NameableToCreate> nameableInserterFactory,
    IDatabaseInserterFactory<CasePartyType.CasePartyTypeToCreate> casePartyTypeInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory,
    TermCreatorFactory nameableDetailsCreatorFactory
) : IEntityCreatorFactory<CasePartyType.CasePartyTypeToCreate>
{
    public async Task<IEntityCreator<CasePartyType.CasePartyTypeToCreate>> CreateAsync(IDbConnection connection) =>
        new NameableCreator<CasePartyType.CasePartyTypeToCreate>(
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
