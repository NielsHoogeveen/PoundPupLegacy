namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class CaseCasePartiesCreatorFactory(
    IDatabaseInserterFactory<CaseParties.ToCreate> casePartiesInserterFactory,
    IDatabaseInserterFactory<CaseCaseParties.ToCreate.ForExistingCase> caseCasePartiesInserterFactory,
    IDatabaseInserterFactory<CasePartiesOrganization> casePartiesOrganizationInserterFactory,
    IDatabaseInserterFactory<CasePartiesPerson> casePartiesPersonInserterFactory
) : IEntityCreatorFactory<CaseCaseParties.ToCreate.ForExistingCase>
{
    public async Task<IEntityCreator<CaseCaseParties.ToCreate.ForExistingCase>> CreateAsync(IDbConnection connection) =>
        new CaseCasePartiesCreator(
            await casePartiesInserterFactory.CreateAsync(connection),
            await caseCasePartiesInserterFactory.CreateAsync(connection),
            await casePartiesOrganizationInserterFactory.CreateAsync(connection),
            await casePartiesPersonInserterFactory.CreateAsync(connection)
        );
}

public class CaseCasePartiesCreator(
    IDatabaseInserter<CaseParties.ToCreate> casePartiesInserter,
    IDatabaseInserter<CaseCaseParties.ToCreate.ForExistingCase> caseCasePartiesInserter,
    IDatabaseInserter<CasePartiesOrganization> casePartiesOrganizationInserter,
    IDatabaseInserter<CasePartiesPerson> casePartiesPersonInserter
) : EntityCreator<CaseCaseParties.ToCreate.ForExistingCase>()
{
    public override async Task ProcessAsync(CaseCaseParties.ToCreate.ForExistingCase element)
    {
        await base.ProcessAsync(element);
        await casePartiesInserter.InsertAsync(element.CaseParties);
        await caseCasePartiesInserter.InsertAsync(element);
        foreach (var organizationId in element.CaseParties.OrganizationIds) {
            await casePartiesOrganizationInserter.InsertAsync(new CasePartiesOrganization {
                CasePartiesId = element.CaseParties.Identification.Id!.Value,
                OrganizationId = organizationId
            });
        }
        foreach (var personId in element.CaseParties.PersonIds) {
            await casePartiesPersonInserter.InsertAsync(new CasePartiesPerson {
                CasePartiesId = element.CaseParties.Identification.Id!.Value,
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
  