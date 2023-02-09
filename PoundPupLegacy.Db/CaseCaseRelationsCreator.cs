namespace PoundPupLegacy.Db;

public class CaseCaseRelationsCreator : IEntityCreator<CaseCaseParties>
{
    public static async Task CreateAsync(IAsyncEnumerable<CaseCaseParties> caseCaseRelationss, NpgsqlConnection connection)
    {

        await using var caseRelationsWriter = await CasePartiesWriter.CreateAsync(connection);
        await using var caseCaseRelationsWriter = await CaseCasePartiesWriter.CreateAsync(connection);
        await using var caseRelationsOrganizationWriter = await CasePartiesOrganizationWriter.CreateAsync(connection);
        await using var caseRelationsPersonWriter = await CasePartiesPersonWriter.CreateAsync(connection);

        await foreach (var caseCaseRelations in caseCaseRelationss)
        {
            await caseRelationsWriter.WriteAsync(caseCaseRelations.CaseParties);
            await caseCaseRelationsWriter.WriteAsync(caseCaseRelations);
            foreach(var organizationId in caseCaseRelations.CaseParties.OrganizationIds)
            {
                await caseRelationsOrganizationWriter.WriteAsync(new CasePartiesOrganization 
                { 
                    CasePartiesId = caseCaseRelations.CaseParties.Id!.Value, 
                    OrganizationId = organizationId 
                });
            }
            foreach (var personId in caseCaseRelations.CaseParties.PersonsIds)
            {
                await caseRelationsPersonWriter.WriteAsync(new CasePartiesPerson
                {
                    CasePartiesId = caseCaseRelations.CaseParties.Id!.Value,
                    PersonId = personId
                });
            }

        }
    }
}
