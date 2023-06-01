namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class PersonCreatorFactory(
    IDatabaseInserterFactory<NodeToCreate> nodeInserterFactory,
    IDatabaseInserterFactory<SearchableToCreate> searchableInserterFactory,
    IDatabaseInserterFactory<DocumentableToCreate> documentableInserterFactory,
    IDatabaseInserterFactory<LocatableToCreate> locatableInserterFactory,
    IDatabaseInserterFactory<NameableToCreate> nameableInserterFactory,
    IDatabaseInserterFactory<PartyToCreate> partyInserterFactory,
    IDatabaseInserterFactory<Person.ToCreate> personInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory,
    TermCreatorFactory nameableDetailsCreatorFactory,
    LocatableDetailsCreatorFactory locatableDetailsCreatorFactory,
    IEntityCreatorFactory<ProfessionalRoleToCreateForExistingPerson> professionalRoleCreatorFactory,
    IEntityCreatorFactory<PersonOrganizationRelation.ToCreate.ForExistingParticipants> personOrganizationRelationCreatorFactory,
    IEntityCreatorFactory<InterPersonalRelation.ToCreate.ForExistingParticipants> interPersonalRelationCreatorFactory,
    IEntityCreatorFactory<PartyPoliticalEntityRelation.ToCreate.ForExistingParty> partyPoliticalRelationCreatorFactory

) : IEntityCreatorFactory<Person.ToCreate>
{
    public async Task<IEntityCreator<Person.ToCreate>> CreateAsync(IDbConnection connection) =>
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
    List<IDatabaseInserter<Person.ToCreate>> inserters,
    NodeDetailsCreator nodeDetailsCreator,
    TermCreator nameableDetailsCreator,
    LocatableDetailsCreator locatableDetailsCreator,
    IEntityCreator<ProfessionalRoleToCreateForExistingPerson> professionalRoleCreator,
    IEntityCreator<PersonOrganizationRelation.ToCreate.ForExistingParticipants> personOrganizationRelationCreator,
    IEntityCreator<InterPersonalRelation.ToCreate.ForExistingParticipants> interPersonalRelationCreator,
    IEntityCreator<PartyPoliticalEntityRelation.ToCreate.ForExistingParty> partyPoliticalRelationCreator
) : 
    LocatableCreator<Person.ToCreate>(inserters, nodeDetailsCreator, nameableDetailsCreator, locatableDetailsCreator)
{
    public override async Task ProcessAsync(Person.ToCreate element, int id)
    {
        await base.ProcessAsync(element, id);
        await professionalRoleCreator
            .CreateAsync(element.PersonDetails.ProfessionalRolesToCreate
                .Select(x => x.ResolvePerson(id))
                .ToAsyncEnumerable());
        await interPersonalRelationCreator
            .CreateAsync(element.PersonDetails.InterPersonalRelationsToCreateFrom
                .Select(x => x.ResolvePersonFrom(id))
                .ToAsyncEnumerable());
        await interPersonalRelationCreator
            .CreateAsync(element.PersonDetails.InterPersonalRelationsToCreateTo
                .Select(x => x.ResolvePersonTo(id))
                .ToAsyncEnumerable());
        await partyPoliticalRelationCreator
            .CreateAsync(element.PersonDetails.PartyPoliticalEntityRelationsToCreate
                .Select(x => x.ResolveParty(id))
                .ToAsyncEnumerable());
        await personOrganizationRelationCreator
            .CreateAsync(element.PersonDetails.PersonOrganizationRelationsToCreate
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