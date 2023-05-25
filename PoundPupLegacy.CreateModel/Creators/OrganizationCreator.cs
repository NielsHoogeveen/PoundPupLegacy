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
    IDatabaseInserterFactory<OrganizationOrganizationType> organizationOrganizationTypeInserterFactory,
    IEntityCreatorFactory<EventuallyIdentifiableInterOrganizationalRelationForExistingParticipants> interOrganizationalRelationCreatorFactory,
    IEntityCreatorFactory<EventuallyIdentifiablePersonOrganizationRelationForExistingParticipants> personOrganizationRelationCreatorFactory,
    IEntityCreatorFactory<EventuallyIdentifiablePartyPoliticalEntityRelationForExistingParty> partyPoliticalRelationCreatorFactory
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
            await organizationOrganizationTypeInserterFactory.CreateAsync(connection),
            await interOrganizationalRelationCreatorFactory.CreateAsync(connection),
            await personOrganizationRelationCreatorFactory.CreateAsync(connection),
            await partyPoliticalRelationCreatorFactory.CreateAsync(connection)
        );
}
public class OrganizationCreator(
    List<IDatabaseInserter<EventuallyIdentifiableOrganization>> inserter,
    NodeDetailsCreator nodeDetailsCreator,
    NameableDetailsCreator nameableDetailsCreator,
    LocatableDetailsCreator locatableDetailsCreator,
    IDatabaseInserter<EventuallyIdentifiableUnitedStatesPoliticalParty> unitedStatesPoliticalPartyInserter,
    IDatabaseInserter<OrganizationOrganizationType> organizationOrganizationTypeInserter,
    IEntityCreator<EventuallyIdentifiableInterOrganizationalRelationForExistingParticipants> interOrganizationalRelationCreator,
    IEntityCreator<EventuallyIdentifiablePersonOrganizationRelationForExistingParticipants> personOrganizationRelationCreator,
    IEntityCreator<EventuallyIdentifiablePartyPoliticalEntityRelationForExistingParty> partyPoliticalRelationCreator
    ) : LocatableCreator<EventuallyIdentifiableOrganization>(inserter, nodeDetailsCreator, nameableDetailsCreator, locatableDetailsCreator) 
{
    public override async Task ProcessAsync(EventuallyIdentifiableOrganization element, int id)
    {
        await base.ProcessAsync(element);
        if (element is NewUnitedStatesPoliticalParty pp) {
            await unitedStatesPoliticalPartyInserter.InsertAsync(pp);
        }
        foreach (var organizationTypeId in element.OrganizationTypeIds) {
            await organizationOrganizationTypeInserter.InsertAsync(new OrganizationOrganizationType{
                OrganizationId = id,
                OrganizationTypeId = organizationTypeId
            });
        }
        await interOrganizationalRelationCreator
            .CreateAsync(element.InterOrganizationalRelationsToAddFrom
                .Select(x => x.ResolveOrganizationFrom(id))
                .ToAsyncEnumerable());
        await interOrganizationalRelationCreator
            .CreateAsync(element.InterOrganizationalRelationsToAddTo
                .Select(x => x.ResolveOrganizationTo(id))
                .ToAsyncEnumerable());
        await partyPoliticalRelationCreator
            .CreateAsync(element.PartyPoliticalEntityRelations
                .Select(x => x.ResolveParty(id))
                .ToAsyncEnumerable());
        await personOrganizationRelationCreator
            .CreateAsync(element.PersonOrganizationRelations
                .Select(x => x.ResolveOrganization(id))
                .ToAsyncEnumerable());
    }
    public override async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();
        await unitedStatesPoliticalPartyInserter.DisposeAsync();
        await organizationOrganizationTypeInserter.DisposeAsync();
        await interOrganizationalRelationCreator.DisposeAsync();
        await personOrganizationRelationCreator.DisposeAsync();
        await partyPoliticalRelationCreator.DisposeAsync();      
    }
}