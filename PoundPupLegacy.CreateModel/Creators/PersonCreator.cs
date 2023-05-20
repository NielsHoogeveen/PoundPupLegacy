namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class PersonCreator(
    IDatabaseInserterFactory<Node> nodeInserterFactory,
    IDatabaseInserterFactory<Searchable> searchableInserterFactory,
    IDatabaseInserterFactory<Documentable> documentableInserterFactory,
    IDatabaseInserterFactory<Locatable> locatableInserterFactory,
    IDatabaseInserterFactory<Nameable> nameableInserterFactory,
    IDatabaseInserterFactory<Party> partyInserterFactory,
    IDatabaseInserterFactory<Person> personInserterFactory,
    IDatabaseInserterFactory<Term> termInserterFactory,
    IMandatorySingleItemDatabaseReaderFactory<TermReaderByNameRequest, Term> termReaderFactory,
    IDatabaseInserterFactory<TermHierarchy> termHierarchyInserterFactory,
    IMandatorySingleItemDatabaseReaderFactory<VocabularyIdReaderByOwnerAndNameRequest, int> vocabularyIdReaderFactory,
    IDatabaseInserterFactory<TenantNode> tenantNodeInserterFactory,
    IEntityCreator<ProfessionalRole> professionalRoleCreator,
    IEntityCreator<PersonOrganizationRelation> personOrganizationRelationCreator
) : EntityCreator<Person>
{
    public override async Task CreateAsync(IAsyncEnumerable<Person> persons, IDbConnection connection)
    {
        await using var nodeWriter = await nodeInserterFactory.CreateAsync(connection);
        await using var searchableWriter = await searchableInserterFactory.CreateAsync(connection);
        await using var documentableWriter = await documentableInserterFactory.CreateAsync(connection);
        await using var locatableWriter = await locatableInserterFactory.CreateAsync(connection);
        await using var nameableWriter = await nameableInserterFactory.CreateAsync(connection);
        await using var partyWriter = await partyInserterFactory.CreateAsync(connection);
        await using var personWriter = await personInserterFactory.CreateAsync(connection);
        await using var termWriter = await termInserterFactory.CreateAsync(connection);
        await using var termReader = await termReaderFactory.CreateAsync(connection);
        await using var termHierarchyWriter = await termHierarchyInserterFactory.CreateAsync(connection);
        await using var vocabularyIdReader = await vocabularyIdReaderFactory.CreateAsync(connection);
        await using var tenantNodeWriter = await tenantNodeInserterFactory.CreateAsync(connection);

        await foreach (var person in persons) {
            await nodeWriter.InsertAsync(person);
            await searchableWriter.InsertAsync(person);
            await documentableWriter.InsertAsync(person);
            await locatableWriter.InsertAsync(person);
            await nameableWriter.InsertAsync(person);
            await partyWriter.InsertAsync(person);
            await personWriter.InsertAsync(person);
            await WriteTerms(person, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in person.TenantNodes) {
                tenantNode.NodeId = person.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }

            foreach (var role in person.ProfessionalRoles) {
                role.PersonId = person.Id;
            }
            await professionalRoleCreator.CreateAsync(person.ProfessionalRoles.ToAsyncEnumerable(), connection);

            foreach (var relation in person.PersonOrganizationRelations) {
                relation.PersonId = person.Id;
            }
            await personOrganizationRelationCreator.CreateAsync(person.PersonOrganizationRelations.ToAsyncEnumerable(), connection);
        }
    }
}
