namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class PersonCreatorFactory(
    IDatabaseInserterFactory<EventuallyIdentifiableNode> nodeInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableSearchable> searchableInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableDocumentable> documentableInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableLocatable> locatableInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableNameable> nameableInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableParty> partyInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiablePerson> personInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory,
    TermCreatorFactory nameableDetailsCreatorFactory,
    LocatableDetailsCreatorFactory locatableDetailsCreatorFactory,
    IEntityCreatorFactory<EventuallyIdentifiableProfessionalRoleForExistingPerson> professionalRoleCreatorFactory,
    IEntityCreatorFactory<EventuallyIdentifiablePersonOrganizationRelationForExistingParticipants> personOrganizationRelationCreatorFactory,
    IEntityCreatorFactory<EventuallyIdentifiableInterPersonalRelationForExistingParticipants> interPersonalRelationCreatorFactory,
    IEntityCreatorFactory<EventuallyIdentifiablePartyPoliticalEntityRelationForExistingParty> partyPoliticalRelationCreatorFactory

) : IEntityCreatorFactory<EventuallyIdentifiablePerson>
{
    public async Task<IEntityCreator<EventuallyIdentifiablePerson>> CreateAsync(IDbConnection connection) =>
        new PersonCreator(
            new() 
            {
                await nodeInserterFactory.CreateAsync(connection),
                await searchableInserterFactory.CreateAsync(connection),
                await documentableInserterFactory.CreateAsync(connection),
                await locatableInserterFactory.CreateAsync(connection),
                await nameableInserterFactory.CreateAsync(connection),
                await partyInserterFactory.CreateAsync(connection),
                await personInserterFactory.CreateAsync(connection)
            },
            await nodeDetailsCreatorFactory.CreateAsync(connection),
            await nameableDetailsCreatorFactory.CreateAsync(connection),
            await locatableDetailsCreatorFactory.CreateAsync(connection),
            await professionalRoleCreatorFactory.CreateAsync(connection),
            await personOrganizationRelationCreatorFactory.CreateAsync(connection),
            await interPersonalRelationCreatorFactory.CreateAsync(connection),
            await partyPoliticalRelationCreatorFactory.CreateAsync(connection)
        );
}

internal sealed class PersonCreator(
    List<IDatabaseInserter<EventuallyIdentifiablePerson>> inserters,
    NodeDetailsCreator nodeDetailsCreator,
    TermCreator nameableDetailsCreator,
    LocatableDetailsCreator locatableDetailsCreator,
    IEntityCreator<EventuallyIdentifiableProfessionalRoleForExistingPerson> professionalRoleCreator,
    IEntityCreator<EventuallyIdentifiablePersonOrganizationRelationForExistingParticipants> personOrganizationRelationCreator,
    IEntityCreator<EventuallyIdentifiableInterPersonalRelationForExistingParticipants> interPersonalRelationCreator,
    IEntityCreator<EventuallyIdentifiablePartyPoliticalEntityRelationForExistingParty> partyPoliticalRelationCreator
) : 
    LocatableCreator<EventuallyIdentifiablePerson>(inserters, nodeDetailsCreator, nameableDetailsCreator, locatableDetailsCreator)
{
    public override async Task ProcessAsync(EventuallyIdentifiablePerson element, int id)
    {
        await base.ProcessAsync(element, id);
        await professionalRoleCreator
            .CreateAsync(element.ProfessionalRoles
                .Select(x => x.ResolvePerson(id))
                .ToAsyncEnumerable());
        await interPersonalRelationCreator
            .CreateAsync(element.InterPersonalRelationsToAddFrom
                .Select(x => x.ResolvePersonFrom(id))
                .ToAsyncEnumerable());
        await interPersonalRelationCreator
            .CreateAsync(element.InterPersonalRelationsToAddTo
                .Select(x => x.ResolvePersonTo(id))
                .ToAsyncEnumerable());
        await partyPoliticalRelationCreator
            .CreateAsync(element.PartyPoliticalEntityRelations
                .Select(x => x.ResolveParty(id))
                .ToAsyncEnumerable());
        await personOrganizationRelationCreator
            .CreateAsync(element.PersonOrganizationRelations
                .Select(x => x.ResolvePerson(id))
                .ToAsyncEnumerable());
    }

    public override async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();
        await professionalRoleCreator.DisposeAsync();
        await personOrganizationRelationCreator.DisposeAsync();
        await interPersonalRelationCreator.DisposeAsync();
        await partyPoliticalRelationCreator.DisposeAsync();
    }
}