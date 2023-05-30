namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class PersonCreatorFactory(
    IDatabaseInserterFactory<NodeToCreate> nodeInserterFactory,
    IDatabaseInserterFactory<SearchableToCreate> searchableInserterFactory,
    IDatabaseInserterFactory<DocumentableToCreate> documentableInserterFactory,
    IDatabaseInserterFactory<LocatableToCreate> locatableInserterFactory,
    IDatabaseInserterFactory<NameableToCreate> nameableInserterFactory,
    IDatabaseInserterFactory<PartyToCreate> partyInserterFactory,
    IDatabaseInserterFactory<Person.PersonToCreate> personInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory,
    TermCreatorFactory nameableDetailsCreatorFactory,
    LocatableDetailsCreatorFactory locatableDetailsCreatorFactory,
    IEntityCreatorFactory<ProfessionalRoleToCreateForExistingPerson> professionalRoleCreatorFactory,
    IEntityCreatorFactory<PersonOrganizationRelation.PersonOrganizationRelationToCreateForExistingParticipants> personOrganizationRelationCreatorFactory,
    IEntityCreatorFactory<InterPersonalRelation.InterPersonalRelationToCreateForExistingParticipants> interPersonalRelationCreatorFactory,
    IEntityCreatorFactory<PartyPoliticalEntityRelation.PartyPoliticalEntityRelationToCreateForExistingParty> partyPoliticalRelationCreatorFactory

) : IEntityCreatorFactory<Person.PersonToCreate>
{
    public async Task<IEntityCreator<Person.PersonToCreate>> CreateAsync(IDbConnection connection) =>
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
    List<IDatabaseInserter<Person.PersonToCreate>> inserters,
    NodeDetailsCreator nodeDetailsCreator,
    TermCreator nameableDetailsCreator,
    LocatableDetailsCreator locatableDetailsCreator,
    IEntityCreator<ProfessionalRoleToCreateForExistingPerson> professionalRoleCreator,
    IEntityCreator<PersonOrganizationRelation.PersonOrganizationRelationToCreateForExistingParticipants> personOrganizationRelationCreator,
    IEntityCreator<InterPersonalRelation.InterPersonalRelationToCreateForExistingParticipants> interPersonalRelationCreator,
    IEntityCreator<PartyPoliticalEntityRelation.PartyPoliticalEntityRelationToCreateForExistingParty> partyPoliticalRelationCreator
) : 
    LocatableCreator<Person.PersonToCreate>(inserters, nodeDetailsCreator, nameableDetailsCreator, locatableDetailsCreator)
{
    public override async Task ProcessAsync(Person.PersonToCreate element, int id)
    {
        await base.ProcessAsync(element, id);
        await professionalRoleCreator
            .CreateAsync(element.PersonDetailsForCreate.ProfessionalRolesToCreate
                .Select(x => x.ResolvePerson(id))
                .ToAsyncEnumerable());
        await interPersonalRelationCreator
            .CreateAsync(element.PersonDetailsForCreate.InterPersonalRelationsToCreateFrom
                .Select(x => x.ResolvePersonFrom(id))
                .ToAsyncEnumerable());
        await interPersonalRelationCreator
            .CreateAsync(element.PersonDetailsForCreate.InterPersonalRelationsToCreateTo
                .Select(x => x.ResolvePersonTo(id))
                .ToAsyncEnumerable());
        await partyPoliticalRelationCreator
            .CreateAsync(element.PersonDetailsForCreate.PartyPoliticalEntityRelationsToCreate
                .Select(x => x.ResolveParty(id))
                .ToAsyncEnumerable());
        await personOrganizationRelationCreator
            .CreateAsync(element.PersonDetailsForCreate.PersonOrganizationRelationToCreate
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