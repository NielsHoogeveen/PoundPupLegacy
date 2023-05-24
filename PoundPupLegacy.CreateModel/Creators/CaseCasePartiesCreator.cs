namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class CaseCasePartiesCreatorFactory(
    IDatabaseInserterFactory<CaseParties> casePartiesInserterFactory,
    IDatabaseInserterFactory<CaseCaseParties> caseCasePartiesInserterFactory,
    IDatabaseInserterFactory<CasePartiesOrganization> casePartiesOrganizationInserterFactory,
    IDatabaseInserterFactory<CasePartiesPerson> casePartiesPersonInserterFactory
) : IEntityCreatorFactory<CaseCaseParties>
{
    public async Task<EntityCreator<CaseCaseParties>> CreateAsync(IDbConnection connection) =>
        new CaseCasePartiesCreator(
            await casePartiesInserterFactory.CreateAsync(connection),
            await caseCasePartiesInserterFactory.CreateAsync(connection),
            await casePartiesOrganizationInserterFactory.CreateAsync(connection),
            await casePartiesPersonInserterFactory.CreateAsync(connection)
        );
}

public class CaseCasePartiesCreator(
    IDatabaseInserter<CaseParties> casePartiesInserter,
    IDatabaseInserter<CaseCaseParties> caseCasePartiesInserter,
    IDatabaseInserter<CasePartiesOrganization> casePartiesOrganizationInserter,
    IDatabaseInserter<CasePartiesPerson> casePartiesPersonInserter
) : EntityCreator<CaseCaseParties>()
{
    public override async Task ProcessAsync(CaseCaseParties element)
    {
        await base.ProcessAsync(element);
        await casePartiesInserter.InsertAsync(element.CaseParties);
        await caseCasePartiesInserter.InsertAsync(element);
        foreach (var organizationId in element.CaseParties.OrganizationIds) {
            await casePartiesOrganizationInserter.InsertAsync(new CasePartiesOrganization {
                CasePartiesId = element.CaseParties.Id!.Value,
                OrganizationId = organizationId
            });
        }
        foreach (var personId in element.CaseParties.PersonsIds) {
            await casePartiesPersonInserter.InsertAsync(new CasePartiesPerson {
                CasePartiesId = element.CaseParties.Id!.Value,
                PersonId = personId
            });
        }
    }
    public override async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();
        await caseCasePartiesInserter.DisposeAsync();
        await casePartiesInserter.DisposeAsync();
        await casePartiesOrganizationInserter.DisposeAsync();
        await casePartiesPersonInserter.DisposeAsync();
    }
}
  