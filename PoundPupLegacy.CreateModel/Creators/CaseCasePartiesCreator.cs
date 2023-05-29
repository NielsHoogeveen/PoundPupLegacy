namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class CaseCasePartiesCreatorFactory(
    IDatabaseInserterFactory<CaseParties.CasePartiesToCreate> casePartiesInserterFactory,
    IDatabaseInserterFactory<CaseExistingCasePartiesToCreate> caseCasePartiesInserterFactory,
    IDatabaseInserterFactory<CasePartiesOrganization> casePartiesOrganizationInserterFactory,
    IDatabaseInserterFactory<CasePartiesPerson> casePartiesPersonInserterFactory
) : IEntityCreatorFactory<CaseExistingCasePartiesToCreate>
{
    public async Task<IEntityCreator<CaseExistingCasePartiesToCreate>> CreateAsync(IDbConnection connection) =>
        new CaseCasePartiesCreator(
            await casePartiesInserterFactory.CreateAsync(connection),
            await caseCasePartiesInserterFactory.CreateAsync(connection),
            await casePartiesOrganizationInserterFactory.CreateAsync(connection),
            await casePartiesPersonInserterFactory.CreateAsync(connection)
        );
}

public class CaseCasePartiesCreator(
    IDatabaseInserter<CaseParties.CasePartiesToCreate> casePartiesInserter,
    IDatabaseInserter<CaseExistingCasePartiesToCreate> caseCasePartiesInserter,
    IDatabaseInserter<CasePartiesOrganization> casePartiesOrganizationInserter,
    IDatabaseInserter<CasePartiesPerson> casePartiesPersonInserter
) : EntityCreator<CaseExistingCasePartiesToCreate>()
{
    public override async Task ProcessAsync(CaseExistingCasePartiesToCreate element)
    {
        await base.ProcessAsync(element);
        await casePartiesInserter.InsertAsync(element.CaseParties);
        await caseCasePartiesInserter.InsertAsync(element);
        foreach (var organizationId in element.CaseParties.OrganizationIds) {
            await casePartiesOrganizationInserter.InsertAsync(new CasePartiesOrganization {
                CasePartiesId = element.CaseParties.IdentificationForCreate.Id!.Value,
                OrganizationId = organizationId
            });
        }
        foreach (var personId in element.CaseParties.PersonIds) {
            await casePartiesPersonInserter.InsertAsync(new CasePartiesPerson {
                CasePartiesId = element.CaseParties.IdentificationForCreate.Id!.Value,
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
  