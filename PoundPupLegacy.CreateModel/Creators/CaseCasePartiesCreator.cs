namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class CaseCasePartiesCreator : EntityCreator<CaseCaseParties>
{
    private readonly IDatabaseInserterFactory<CaseParties> _casePartiesInserterFactory;
    private readonly IDatabaseInserterFactory<CaseCaseParties> _caseCasePartiesInserterFactory;
    private readonly IDatabaseInserterFactory<CasePartiesOrganization> _casePartiesOrganizationInserterFactory;
    private readonly IDatabaseInserterFactory<CasePartiesPerson> _casePartiesPersonInserterFactory;
    public CaseCasePartiesCreator(
        IDatabaseInserterFactory<CaseParties> casePartiesInserterFactory,
        IDatabaseInserterFactory<CaseCaseParties> caseCasePartiesInserterFactory,
        IDatabaseInserterFactory<CasePartiesOrganization> casePartiesOrganizationInserterFactory,
        IDatabaseInserterFactory<CasePartiesPerson> casePartiesPersonInserterFactory
        )
    {
        _casePartiesInserterFactory = casePartiesInserterFactory;
        _casePartiesPersonInserterFactory = casePartiesPersonInserterFactory;
        _caseCasePartiesInserterFactory = caseCasePartiesInserterFactory;
        _casePartiesOrganizationInserterFactory = casePartiesOrganizationInserterFactory;
    }
    public override async Task CreateAsync(IAsyncEnumerable<CaseCaseParties> caseCaseRelationss, IDbConnection connection)
    {

        await using var caseRelationsWriter = await _casePartiesInserterFactory.CreateAsync(connection);
        await using var caseCaseRelationsWriter = await _caseCasePartiesInserterFactory.CreateAsync(connection);
        await using var caseRelationsOrganizationWriter = await _casePartiesOrganizationInserterFactory.CreateAsync(connection);
        await using var caseRelationsPersonWriter = await _casePartiesPersonInserterFactory.CreateAsync(connection);

        await foreach (var caseCaseRelations in caseCaseRelationss) {
            await caseRelationsWriter.InsertAsync(caseCaseRelations.CaseParties);
            await caseCaseRelationsWriter.InsertAsync(caseCaseRelations);
            foreach (var organizationId in caseCaseRelations.CaseParties.OrganizationIds) {
                await caseRelationsOrganizationWriter.InsertAsync(new CasePartiesOrganization {
                    CasePartiesId = caseCaseRelations.CaseParties.Id!.Value,
                    OrganizationId = organizationId
                });
            }
            foreach (var personId in caseCaseRelations.CaseParties.PersonsIds) {
                await caseRelationsPersonWriter.InsertAsync(new CasePartiesPerson {
                    CasePartiesId = caseCaseRelations.CaseParties.Id!.Value,
                    PersonId = personId
                });
            }

        }
    }
}
