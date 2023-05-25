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
    NameableDetailsCreatorFactory nameableDetailsCreatorFactory,
    LocatableDetailsCreatorFactory locatableDetailsCreatorFactory,
    IEntityCreatorFactory<EventuallyIdentifiableProfessionalRole> professionalRoleCreatorFactory,
    IEntityCreatorFactory<EventuallyIdentifiablePersonOrganizationRelationForNewPerson> personOrganizationRelationCreatorFactory
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
            await personOrganizationRelationCreatorFactory.CreateAsync(connection)
        );
}

internal sealed class PersonCreator(
    List<IDatabaseInserter<EventuallyIdentifiablePerson>> inserters,
    NodeDetailsCreator nodeDetailsCreator,
    NameableDetailsCreator nameableDetailsCreator,
    LocatableDetailsCreator locatableDetailsCreator,
    IEntityCreator<EventuallyIdentifiableProfessionalRole> professionalRoleCreator,
    IEntityCreator<EventuallyIdentifiablePersonOrganizationRelationForNewPerson> personOrganizationRelationCreator
) : 
    LocatableCreator<EventuallyIdentifiablePerson>(inserters, nodeDetailsCreator, nameableDetailsCreator, locatableDetailsCreator)
{
    public override async Task ProcessAsync(EventuallyIdentifiablePerson element)
    {
        await base.ProcessAsync(element);
        foreach (var role in element.ProfessionalRoles) {
            role.PersonId = element.Id;
        }
        await professionalRoleCreator.CreateAsync(element.ProfessionalRoles.ToAsyncEnumerable());

        foreach (var relation in element.PersonOrganizationRelations) {
            relation.PersonId = element.Id;
        }
        await personOrganizationRelationCreator.CreateAsync(element.PersonOrganizationRelations.ToAsyncEnumerable());
    }

    public override async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();
        await professionalRoleCreator.DisposeAsync();
        await personOrganizationRelationCreator.DisposeAsync();
    }
}