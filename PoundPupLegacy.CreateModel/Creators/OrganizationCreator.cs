namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class OrganizationCreatorFactory(
    IDatabaseInserterFactory<NodeToCreate> nodeIntererFactory,
    IDatabaseInserterFactory<SearchableToCreate> searchableInserterFactory,
    IDatabaseInserterFactory<DocumentableToCreate> documentableInserterFactory,
    IDatabaseInserterFactory<LocatableToCreate> locatableInserterFactory,
    IDatabaseInserterFactory<NameableToCreate> nameableInserterFactory,
    IDatabaseInserterFactory<PartyToCreate> partyInserterFactory,
    IDatabaseInserterFactory<OrganizationToCreate> organizationInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory,
    TermCreatorFactory nameableDetailsCreatorFactory,
    LocatableDetailsCreatorFactory locatableDetailsCreatorFactory,
    IDatabaseInserterFactory<UnitedStatesPoliticalParty.ToCreate> unitedStatesPoliticalPartyInserterFactory,
    IDatabaseInserterFactory<OrganizationOrganizationType> organizationOrganizationTypeInserterFactory,
    IEntityCreatorFactory<InterOrganizationalRelation.ToCreateForExistingParticipants> interOrganizationalRelationCreatorFactory,
    IEntityCreatorFactory<PersonOrganizationRelation.ToCreateForExistingParticipants> personOrganizationRelationCreatorFactory,
    IEntityCreatorFactory<PartyPoliticalEntityRelation.ToCreateForExistingParty> partyPoliticalRelationCreatorFactory
) : IEntityCreatorFactory<OrganizationToCreate>
{
    public async Task<IEntityCreator<OrganizationToCreate>> CreateAsync(IDbConnection connection) =>
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
    List<IDatabaseInserter<OrganizationToCreate>> inserter,
    NodeDetailsCreator nodeDetailsCreator,
    TermCreator nameableDetailsCreator,
    LocatableDetailsCreator locatableDetailsCreator,
    IDatabaseInserter<UnitedStatesPoliticalParty.ToCreate> unitedStatesPoliticalPartyInserter,
    IDatabaseInserter<OrganizationOrganizationType> organizationOrganizationTypeInserter,
    IEntityCreator<InterOrganizationalRelation.ToCreateForExistingParticipants> interOrganizationalRelationCreator,
    IEntityCreator<PersonOrganizationRelation.ToCreateForExistingParticipants> personOrganizationRelationCreator,
    IEntityCreator<PartyPoliticalEntityRelation.ToCreateForExistingParty> partyPoliticalRelationCreator
    ) : LocatableCreator<OrganizationToCreate>(inserter, nodeDetailsCreator, nameableDetailsCreator, locatableDetailsCreator) 
{
    public override async Task ProcessAsync(OrganizationToCreate element, int id)
    {
        await base.ProcessAsync(element, id);
        if (element is UnitedStatesPoliticalParty.ToCreate pp) {
            await unitedStatesPoliticalPartyInserter.InsertAsync(pp);
        }
        foreach (var organizationTypeId in element.OrganizationDetails.OrganizationTypeIds) {
            await organizationOrganizationTypeInserter.InsertAsync(new OrganizationOrganizationType{
                OrganizationId = id,
                OrganizationTypeId = organizationTypeId
            });
        }
        await interOrganizationalRelationCreator
            .CreateAsync(element.OrganizationDetails.InterOrganizationalRelationsFrom
                .Select(x => x.ResolveOrganizationFrom(id))
                .ToAsyncEnumerable());
        await interOrganizationalRelationCreator
            .CreateAsync(element.OrganizationDetails.InterOrganizationalRelationsTo
                .Select(x => x.ResolveOrganizationTo(id))
                .ToAsyncEnumerable());
        await partyPoliticalRelationCreator
            .CreateAsync(element.OrganizationDetails.PartyPoliticalEntityRelationsToCreate
                .Select(x => x.ResolveParty(id))
                .ToAsyncEnumerable());
        await personOrganizationRelationCreator
            .CreateAsync(element.OrganizationDetails.PersonOrganizationRelationsToCreate
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