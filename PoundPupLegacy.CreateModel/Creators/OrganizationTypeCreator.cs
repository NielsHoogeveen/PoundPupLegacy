namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class OrganizationTypeCreator(
    IDatabaseInserterFactory<Node> nodeInserterFactory,
    IDatabaseInserterFactory<Searchable> searchableInserterFactory,
    IDatabaseInserterFactory<Nameable> nameableInserterFactory,
    IDatabaseInserterFactory<NewOrganizationType> organizationTypeInserterFactory,
    IDatabaseInserterFactory<Term> termInserterFactory,
    IMandatorySingleItemDatabaseReaderFactory<TermReaderByNameRequest, Term> termReaderFactory,
    IDatabaseInserterFactory<TermHierarchy> termHierarchyInserterFactory,
    IMandatorySingleItemDatabaseReaderFactory<VocabularyIdReaderByOwnerAndNameRequest, int> vocabularyIdReaderFactory,
    IDatabaseInserterFactory<TenantNode> tenantNodeInserterFactory
) : EntityCreator<NewOrganizationType>
{
    public override async Task CreateAsync(IAsyncEnumerable<NewOrganizationType> organizationTypes, IDbConnection connection)
    {
        await using var nodeWriter = await nodeInserterFactory.CreateAsync(connection);
        await using var searchableWriter = await searchableInserterFactory.CreateAsync(connection);
        await using var nameableWriter = await nameableInserterFactory.CreateAsync(connection);
        await using var organizationTypeWriter = await organizationTypeInserterFactory.CreateAsync(connection);
        await using var termWriter = await termInserterFactory.CreateAsync(connection);
        await using var termReader = await termReaderFactory.CreateAsync(connection);
        await using var termHierarchyWriter = await termHierarchyInserterFactory.CreateAsync(connection);
        await using var vocabularyIdReader = await vocabularyIdReaderFactory.CreateAsync(connection);
        await using var tenantNodeWriter = await tenantNodeInserterFactory.CreateAsync(connection);

        await foreach (var organizationType in organizationTypes) {
            await nodeWriter.InsertAsync(organizationType);
            await searchableWriter.InsertAsync(organizationType);
            await nameableWriter.InsertAsync(organizationType);
            await organizationTypeWriter.InsertAsync(organizationType);
            await WriteTerms(organizationType, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in organizationType.TenantNodes) {
                tenantNode.NodeId = organizationType.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }
        }
    }
}
