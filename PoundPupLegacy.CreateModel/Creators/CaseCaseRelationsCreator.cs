namespace PoundPupLegacy.CreateModel.Creators;

public class CaseCaseRelationsCreator : IEntityCreator<CaseCaseParties>
{
    public static async Task CreateAsync(IAsyncEnumerable<CaseCaseParties> caseCaseRelationss, NpgsqlConnection connection)
    {

        await using var caseRelationsWriter = await CasePartiesInserter.CreateAsync(connection);
        await using var caseCaseRelationsWriter = await CaseCasePartiesInserter.CreateAsync(connection);
        await using var caseRelationsOrganizationWriter = await CasePartiesOrganizationInserter.CreateAsync(connection);
        await using var caseRelationsPersonWriter = await CasePartiesPersonInserter.CreateAsync(connection);

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
