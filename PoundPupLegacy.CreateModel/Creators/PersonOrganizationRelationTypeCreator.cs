namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class PersonOrganizationRelationTypeCreator(
    IDatabaseInserterFactory<Node> nodeInserterFactory,
    IDatabaseInserterFactory<Searchable> searchableInserterFactory,
    IDatabaseInserterFactory<Nameable> nameableInserterFactory,
    IDatabaseInserterFactory<NewPersonOrganizationRelationType> personOrganizationRelationTypeInserterFactory,
    IDatabaseInserterFactory<Term> termInserterFactory,
    IMandatorySingleItemDatabaseReaderFactory<TermReaderByNameRequest, Term> termReaderFactory,
    IDatabaseInserterFactory<TermHierarchy> termHierarchyInserterFactory,
    IMandatorySingleItemDatabaseReaderFactory<VocabularyIdReaderByOwnerAndNameRequest, int> vocabularyIdReaderFactory,
    IDatabaseInserterFactory<TenantNode> tenantNodeInserterFactory
) : EntityCreator<NewPersonOrganizationRelationType>
{
    public override async Task CreateAsync(IAsyncEnumerable<NewPersonOrganizationRelationType> personOrganizationRelationTypes, IDbConnection connection)
    {
        await using var nodeWriter = await nodeInserterFactory.CreateAsync(connection);
        await using var searchableWriter = await searchableInserterFactory.CreateAsync(connection);
        await using var nameableWriter = await nameableInserterFactory.CreateAsync(connection);
        await using var personOrganizationRelationTypeWriter = await personOrganizationRelationTypeInserterFactory.CreateAsync(connection);
        await using var termWriter = await termInserterFactory.CreateAsync(connection);
        await using var termReader = await termReaderFactory.CreateAsync(connection);
        await using var termHierarchyWriter = await termHierarchyInserterFactory.CreateAsync(connection);
        await using var vocabularyIdReader = await vocabularyIdReaderFactory.CreateAsync(connection);
        await using var tenantNodeWriter = await tenantNodeInserterFactory.CreateAsync(connection);

        await foreach (var personOrganizationRelationType in personOrganizationRelationTypes) {
            await nodeWriter.InsertAsync(personOrganizationRelationType);
            await searchableWriter.InsertAsync(personOrganizationRelationType);
            await nameableWriter.InsertAsync(personOrganizationRelationType);
            await personOrganizationRelationTypeWriter.InsertAsync(personOrganizationRelationType);
            await WriteTerms(personOrganizationRelationType, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in personOrganizationRelationType.TenantNodes) {
                tenantNode.NodeId = personOrganizationRelationType.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }
        }
    }
}
