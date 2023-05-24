namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class OrganizationCreatorFactory(
    IDatabaseInserterFactory<EventuallyIdentifiableNode> nodeIntererFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableSearchable> searchableInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableDocumentable> documentableInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableLocatable> locatableInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableNameable> nameableInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableParty> partyInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableOrganization> organizationInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory,
    NameableDetailsCreatorFactory nameableDetailsCreatorFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableUnitedStatesPoliticalParty> unitedStatesPoliticalPartyInserterFactory,
    IDatabaseInserterFactory<OrganizationOrganizationType> organizationOrganizationTypeInserterFactory
) : INameableCreatorFactory<EventuallyIdentifiableOrganization>
{
    public async Task<NameableCreator<EventuallyIdentifiableOrganization>> CreateAsync(IDbConnection connection) =>
        new OrganizationCreator(
            new ()
            {
                await nodeIntererFactory.CreateAsync(connection),
                await searchableInserterFactory.CreateAsync(connection),
                await documentableInserterFactory.CreateAsync(connection),
                await locatableInserterFactory.CreateAsync(connection),
                await nameableInserterFactory.CreateAsync(connection),
                await partyInserterFactory.CreateAsync(connection),
                await organizationInserterFactory.CreateAsync(connection)
            },
            await nodeDetailsCreatorFactory.CreateAsync(connection),
            await nameableDetailsCreatorFactory.CreateAsync(connection),
            await unitedStatesPoliticalPartyInserterFactory.CreateAsync(connection),
            await organizationOrganizationTypeInserterFactory.CreateAsync(connection)
        );
}
public class OrganizationCreator(
    List<IDatabaseInserter<EventuallyIdentifiableOrganization>> inserter,
    NodeDetailsCreator nodeDetailsCreator,
    NameableDetailsCreator nameableDetailsCreator,
    IDatabaseInserter<EventuallyIdentifiableUnitedStatesPoliticalParty> unitedStatesPoliticalPartyInserter,
    IDatabaseInserter<OrganizationOrganizationType> organizationOrganizationTypeInserter

    ) : NameableCreator<EventuallyIdentifiableOrganization>(inserter, nodeDetailsCreator, nameableDetailsCreator) 
{
    public override async Task ProcessAsync(EventuallyIdentifiableOrganization element)
    {
        await base.ProcessAsync(element);
        if (element is NewUnitedStatesPoliticalParty pp) {
            await unitedStatesPoliticalPartyInserter.InsertAsync(pp);
        }
        foreach (var organizationOrganizationType in element.OrganizationTypes) {
            organizationOrganizationType.OrganizationId = element.Id;
            await organizationOrganizationTypeInserter.InsertAsync(organizationOrganizationType);
        }

    }
    public override async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();
        await unitedStatesPoliticalPartyInserter.DisposeAsync();
        await organizationOrganizationTypeInserter.DisposeAsync();
    }
}