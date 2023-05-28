namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class CaseCasePartiesCreatorFactory(
    IDatabaseInserterFactory<NewCaseParties> casePartiesInserterFactory,
    IDatabaseInserterFactory<CaseNewCasePartiesToUpdate> caseCasePartiesInserterFactory,
    IDatabaseInserterFactory<CasePartiesOrganization> casePartiesOrganizationInserterFactory,
    IDatabaseInserterFactory<CasePartiesPerson> casePartiesPersonInserterFactory
) : IEntityCreatorFactory<CaseNewCasePartiesToUpdate>
{
    public async Task<IEntityCreator<CaseNewCasePartiesToUpdate>> CreateAsync(IDbConnection connection) =>
        new CaseCasePartiesCreator(
            await casePartiesInserterFactory.CreateAsync(connection),
            await caseCasePartiesInserterFactory.CreateAsync(connection),
            await casePartiesOrganizationInserterFactory.CreateAsync(connection),
            await casePartiesPersonInserterFactory.CreateAsync(connection)
        );
}

public class CaseCasePartiesCreator(
    IDatabaseInserter<NewCaseParties> casePartiesInserter,
    IDatabaseInserter<CaseNewCasePartiesToUpdate> caseCasePartiesInserter,
    IDatabaseInserter<CasePartiesOrganization> casePartiesOrganizationInserter,
    IDatabaseInserter<CasePartiesPerson> casePartiesPersonInserter
) : EntityCreator<CaseNewCasePartiesToUpdate>()
{
    public override async Task ProcessAsync(CaseNewCasePartiesToUpdate element)
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
        foreach (var personId in element.CaseParties.PersonIds) {
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
  