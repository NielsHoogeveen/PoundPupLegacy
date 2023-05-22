namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class InterOrganizationalRelationTypeCreator(
    IDatabaseInserterFactory<Node> nodeInserterFactory,
    IDatabaseInserterFactory<Searchable> searchableInserterFactory,
    IDatabaseInserterFactory<Nameable> nameableInserterFactory,
    IDatabaseInserterFactory<NewInterOrganizationalRelationType> interOrganizationalRelationTypeInserterFactory,
    IDatabaseInserterFactory<Term> termInserterFactory,
    IMandatorySingleItemDatabaseReaderFactory<TermReaderByNameRequest, Term> termReaderFactory,
    IDatabaseInserterFactory<TermHierarchy> termHierarchyInserterFactory,
    IMandatorySingleItemDatabaseReaderFactory<VocabularyIdReaderByOwnerAndNameRequest, int> vocabularyIdReaderFactory,
    IDatabaseInserterFactory<TenantNode> tenantNodeInserterFactory
) : EntityCreator<NewInterOrganizationalRelationType>
{
    public override async Task CreateAsync(IAsyncEnumerable<NewInterOrganizationalRelationType> interOrganizationalRelationTypes, IDbConnection connection)
    {
        await using var nodeWriter = await nodeInserterFactory.CreateAsync(connection);
        await using var searchableWriter = await searchableInserterFactory.CreateAsync(connection);
        await using var nameableWriter = await nameableInserterFactory.CreateAsync(connection);
        await using var interOrganizationalRelationTypeWriter = await interOrganizationalRelationTypeInserterFactory.CreateAsync(connection);
        await using var termWriter = await termInserterFactory.CreateAsync(connection);
        await using var termReader = await termReaderFactory.CreateAsync(connection);
        await using var termHierarchyWriter = await termHierarchyInserterFactory.CreateAsync(connection);
        await using var vocabularyIdReader = await vocabularyIdReaderFactory.CreateAsync(connection);
        await using var tenantNodeWriter = await tenantNodeInserterFactory.CreateAsync(connection);

        await foreach (var interOrganizationalRelationType in interOrganizationalRelationTypes) {
            await nodeWriter.InsertAsync(interOrganizationalRelationType);
            await searchableWriter.InsertAsync(interOrganizationalRelationType);
            await nameableWriter.InsertAsync(interOrganizationalRelationType);
            await interOrganizationalRelationTypeWriter.InsertAsync(interOrganizationalRelationType);
            await WriteTerms(interOrganizationalRelationType, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in interOrganizationalRelationType.TenantNodes) {
                tenantNode.NodeId = interOrganizationalRelationType.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }
        }
    }
}
