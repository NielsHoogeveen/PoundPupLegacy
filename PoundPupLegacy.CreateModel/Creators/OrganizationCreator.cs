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
    LocatableDetailsCreatorFactory locatableDetailsCreatorFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableUnitedStatesPoliticalParty> unitedStatesPoliticalPartyInserterFactory,
    IDatabaseInserterFactory<OrganizationOrganizationType> organizationOrganizationTypeInserterFactory
) : IEntityCreatorFactory<EventuallyIdentifiableOrganization>
{
    public async Task<IEntityCreator<EventuallyIdentifiableOrganization>> CreateAsync(IDbConnection connection) =>
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
            await locatableDetailsCreatorFactory.CreateAsync(connection),
            await unitedStatesPoliticalPartyInserterFactory.CreateAsync(connection),
            await organizationOrganizationTypeInserterFactory.CreateAsync(connection)
        );
}
public class OrganizationCreator(
    List<IDatabaseInserter<EventuallyIdentifiableOrganization>> inserter,
    NodeDetailsCreator nodeDetailsCreator,
    NameableDetailsCreator nameableDetailsCreator,
    LocatableDetailsCreator locatableDetailsCreator,
    IDatabaseInserter<EventuallyIdentifiableUnitedStatesPoliticalParty> unitedStatesPoliticalPartyInserter,
    IDatabaseInserter<OrganizationOrganizationType> organizationOrganizationTypeInserter

    ) : LocatableCreator<EventuallyIdentifiableOrganization>(inserter, nodeDetailsCreator, nameableDetailsCreator, locatableDetailsCreator) 
{
    public override async Task ProcessAsync(EventuallyIdentifiableOrganization element)
    {
        await base.ProcessAsync(element);
        if (element is NewUnitedStatesPoliticalParty pp) {
            await unitedStatesPoliticalPartyInserter.InsertAsync(pp);
        }
        foreach (var organizationTypeId in element.OrganizationTypeIds) {
            await organizationOrganizationTypeInserter.InsertAsync(new OrganizationOrganizationType{
                OrganizationId = element.Id,
                OrganizationTypeId = organizationTypeId
            });
        }

    }
    public override async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();
        await unitedStatesPoliticalPartyInserter.DisposeAsync();
        await organizationOrganizationTypeInserter.DisposeAsync();
    }
}