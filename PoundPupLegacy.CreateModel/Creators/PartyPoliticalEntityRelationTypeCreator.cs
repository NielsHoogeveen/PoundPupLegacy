namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class PartyPoliticalEntityRelationTypeCreator(
    IDatabaseInserterFactory<Node> nodeInserterFactory,
    IDatabaseInserterFactory<Searchable> searchableInserterFactory,
    IDatabaseInserterFactory<Nameable> nameableInserterFactory,
    IDatabaseInserterFactory<PartyPoliticalEntityRelationType> politicalEntityRelationTypeInserterFactory,
    IDatabaseInserterFactory<Term> termInserterFactory,
    IMandatorySingleItemDatabaseReaderFactory<TermReaderByNameRequest, Term> termReaderFactory,
    IDatabaseInserterFactory<TermHierarchy> termHierarchyInserterFactory,
    IMandatorySingleItemDatabaseReaderFactory<VocabularyIdReaderByOwnerAndNameRequest, int> vocabularyIdReaderFactory,
    IDatabaseInserterFactory<TenantNode> tenantNodeInserterFactory
) : EntityCreator<PartyPoliticalEntityRelationType>
{
    public override async Task CreateAsync(IAsyncEnumerable<PartyPoliticalEntityRelationType> politicalEntityRelationTypes, IDbConnection connection)
    {
        await using var nodeWriter = await nodeInserterFactory.CreateAsync(connection);
        await using var searchableWriter = await searchableInserterFactory.CreateAsync(connection);
        await using var nameableWriter = await nameableInserterFactory.CreateAsync(connection);
        await using var politicalEntityRelationTypeWriter = await politicalEntityRelationTypeInserterFactory.CreateAsync(connection);
        await using var termWriter = await termInserterFactory.CreateAsync(connection);
        await using var termReader = await termReaderFactory.CreateAsync(connection);
        await using var termHierarchyWriter = await termHierarchyInserterFactory.CreateAsync(connection);
        await using var vocabularyIdReader = await vocabularyIdReaderFactory.CreateAsync(connection);
        await using var tenantNodeWriter = await tenantNodeInserterFactory.CreateAsync(connection);

        await foreach (var politicalEntityRelationType in politicalEntityRelationTypes) {
            await nodeWriter.InsertAsync(politicalEntityRelationType);
            await searchableWriter.InsertAsync(politicalEntityRelationType);
            await nameableWriter.InsertAsync(politicalEntityRelationType);
            await politicalEntityRelationTypeWriter.InsertAsync(politicalEntityRelationType);
            await WriteTerms(politicalEntityRelationType, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in politicalEntityRelationType.TenantNodes) {
                tenantNode.NodeId = politicalEntityRelationType.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }
        }
    }
}
