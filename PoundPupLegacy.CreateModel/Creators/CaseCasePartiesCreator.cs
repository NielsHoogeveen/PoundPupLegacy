namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class CaseCasePartiesCreator(
    IDatabaseInserterFactory<CaseParties> casePartiesInserterFactory,
    IDatabaseInserterFactory<CaseCaseParties> caseCasePartiesInserterFactory,
    IDatabaseInserterFactory<CasePartiesOrganization> casePartiesOrganizationInserterFactory,
    IDatabaseInserterFactory<CasePartiesPerson> casePartiesPersonInserterFactory
) : EntityCreator<CaseCaseParties>
{
    public override async Task CreateAsync(IAsyncEnumerable<CaseCaseParties> caseCaseRelationss, IDbConnection connection)
    {
        await using var caseRelationsWriter = await casePartiesInserterFactory.CreateAsync(connection);
        await using var caseCaseRelationsWriter = await caseCasePartiesInserterFactory.CreateAsync(connection);
        await using var caseRelationsOrganizationWriter = await casePartiesOrganizationInserterFactory.CreateAsync(connection);
        await using var caseRelationsPersonWriter = await casePartiesPersonInserterFactory.CreateAsync(connection);

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
