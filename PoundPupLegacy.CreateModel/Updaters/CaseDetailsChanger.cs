using PoundPupLegacy.DomainModel.Creators;
using PoundPupLegacy.DomainModel.Deleters;
using static PoundPupLegacy.DomainModel.CaseDetails;

namespace PoundPupLegacy.DomainModel.Updaters;

public class CaseDetailsChangerFactory(
    IDatabaseUpdaterFactory<CaseParties.ToUpdate> casePartyUpdaterFactory,
    IDatabaseDeleterFactory<CasePartiesOrganizationDeleterRequest> organizationDeleterFactory,
    IDatabaseDeleterFactory<CasePartiesPersonDeleterRequest> personDeleterFactory,
    IDatabaseInserterFactory<CaseParties.ToCreate> casePartiesInserterFactory,
    IDatabaseInserterFactory<CasePartiesOrganization> casePartiesOrganizationInserterFactory,
    IDatabaseInserterFactory<CasePartiesPerson> casePartiesPersonInserterFactory,
    IEntityCreatorFactory<CaseCaseParties.ToCreate.ForExistingCase> caseCasePartiesCreatorFactory
)
{
    public async Task<CaseDetailsChanger> CreateAsync(IDbConnection connection)
    {
        return new CaseDetailsChanger(
            await casePartyUpdaterFactory.CreateAsync(connection),
            await organizationDeleterFactory.CreateAsync(connection),
            await personDeleterFactory.CreateAsync(connection),
            await casePartiesInserterFactory.CreateAsync(connection),
            await casePartiesOrganizationInserterFactory.CreateAsync(connection),
            await casePartiesPersonInserterFactory.CreateAsync(connection),
            await caseCasePartiesCreatorFactory.CreateAsync(connection)
        );
    }
}
public class CaseDetailsChanger(
    IDatabaseUpdater<CaseParties.ToUpdate> casePartyUpdater,
    IDatabaseDeleter<CasePartiesOrganizationDeleterRequest> organizationDeleter,
    IDatabaseDeleter<CasePartiesPersonDeleterRequest> personDeleter,
    IDatabaseInserter<CaseParties.ToCreate> casePartiesInserter,
    IDatabaseInserter<CasePartiesOrganization> casePartiesOrganizationInserter,
    IDatabaseInserter<CasePartiesPerson> casePartiesPersonInserter,
    IEntityCreator<CaseCaseParties.ToCreate.ForExistingCase> caseCasePartiesCreator)
    : IAsyncDisposable
{
    public async Task Process(CaseToUpdate caseDetails)
    {
        foreach (var caseParty in caseDetails.CaseDetails.CasePartiesToAdd) {
            await caseCasePartiesCreator.CreateAsync(caseParty);
        }
        foreach (var caseParty in caseDetails.CaseDetails.CasePartiesToUpdate) {
            await casePartyUpdater.UpdateAsync(caseParty.CaseParties);
            foreach (var organizationId in caseParty.CaseParties.OrganizationIdsToRemove) {
                await organizationDeleter.DeleteAsync(new CasePartiesOrganizationDeleterRequest {
                    CasePartiesId = caseParty.CaseParties.Identification.Id,
                    OrganizationId = organizationId
                });
            }
            foreach (var personId in caseParty.CaseParties.PersonIdsToRemove) {
                await personDeleter.DeleteAsync(new CasePartiesPersonDeleterRequest {
                    CasePartiesId = caseParty.CaseParties.Identification.Id,
                    PersonId = personId
                });
            }
            foreach (var organizationId in caseParty.CaseParties.OrganizationIdsToAdd) {
                await casePartiesOrganizationInserter.InsertAsync(new CasePartiesOrganization {
                    CasePartiesId = caseParty.CaseParties.Identification.Id,
                    OrganizationId = organizationId
                });
            }
            foreach (var personId in caseParty.CaseParties.PersonIdsToAdd) {
                await casePartiesPersonInserter.InsertAsync(new CasePartiesPerson {
                    CasePartiesId = caseParty.CaseParties.Identification.Id,
                    PersonId = personId
                });
            }
        }
    }
    public async ValueTask DisposeAsync()
    {
        await casePartyUpdater.DisposeAsync();
        await organizationDeleter.DisposeAsync();
        await personDeleter.DisposeAsync();
        await casePartiesInserter.DisposeAsync();
        await casePartiesOrganizationInserter.DisposeAsync();
        await casePartiesPersonInserter.DisposeAsync();
        await caseCasePartiesCreator.DisposeAsync();
    }

}


internal sealed class CasePartiesUpdaterFactory : DatabaseUpdaterFactory<CaseParties.ToUpdate>
{
    private static readonly NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    private static readonly NullableStringDatabaseParameter OrganizationsText = new() { Name = "organizations" };
    private static readonly NullableStringDatabaseParameter PersonsText = new() { Name = "persons" };

    public override string Sql => $"""
        update case_parties set organizations=@organizations, persons=@persons
        where id = @id;
        """;
    protected override IEnumerable<ParameterValue> GetParameterValues(CaseParties.ToUpdate request)
    {
        return new List<ParameterValue> {
            ParameterValue.Create(Id, request.Identification.Id),
            ParameterValue.Create(OrganizationsText, request.Organizations),
            ParameterValue.Create(PersonsText, request.Persons),
        };
    }
}

