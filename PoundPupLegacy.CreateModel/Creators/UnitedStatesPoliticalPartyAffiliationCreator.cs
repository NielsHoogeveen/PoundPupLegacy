namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class UnitedStatesPoliticalPartyAffliationCreator(
    IDatabaseInserterFactory<Node> nodeInserterFactory,
    IDatabaseInserterFactory<Searchable> searchableInserterFactory,
    IDatabaseInserterFactory<Nameable> nameableInserterFactory,
    IDatabaseInserterFactory<UnitedStatesPoliticalPartyAffliation> unitedStatesPoliticalPartyAffliationInserterFactory,
    IDatabaseInserterFactory<Term> termInserterFactory,
    IMandatorySingleItemDatabaseReaderFactory<TermReaderByNameRequest, Term> termReaderFactory,
    IDatabaseInserterFactory<TermHierarchy> termHierarchyInserterFactory,
    IMandatorySingleItemDatabaseReaderFactory<VocabularyIdReaderByOwnerAndNameRequest, int> vocabularyIdReaderFactory,
    IDatabaseInserterFactory<TenantNode> tenantNodeInserterFactory
) : EntityCreator<UnitedStatesPoliticalPartyAffliation>
{
    public override async Task CreateAsync(IAsyncEnumerable<UnitedStatesPoliticalPartyAffliation> unitedStatesPoliticalPartyAffliations, IDbConnection connection)
    {
        await using var nodeWriter = await nodeInserterFactory.CreateAsync(connection);
        await using var searchableWriter = await searchableInserterFactory.CreateAsync(connection);
        await using var nameableWriter = await nameableInserterFactory.CreateAsync(connection);
        await using var unitedStatesPoliticalPartyAffliationWriter = await unitedStatesPoliticalPartyAffliationInserterFactory.CreateAsync(connection);
        await using var termWriter = await termInserterFactory.CreateAsync(connection);
        await using var termReader = await termReaderFactory.CreateAsync(connection);
        await using var termHierarchyWriter = await termHierarchyInserterFactory.CreateAsync(connection);
        await using var vocabularyIdReader = await vocabularyIdReaderFactory.CreateAsync(connection);
        await using var tenantNodeWriter = await tenantNodeInserterFactory.CreateAsync(connection);

        await foreach (var unitedStatesPoliticalPartyAffliation in unitedStatesPoliticalPartyAffliations) {
            await nodeWriter.InsertAsync(unitedStatesPoliticalPartyAffliation);
            await searchableWriter.InsertAsync(unitedStatesPoliticalPartyAffliation);
            await nameableWriter.InsertAsync(unitedStatesPoliticalPartyAffliation);
            await unitedStatesPoliticalPartyAffliationWriter.InsertAsync(unitedStatesPoliticalPartyAffliation);
            await WriteTerms(unitedStatesPoliticalPartyAffliation, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in unitedStatesPoliticalPartyAffliation.TenantNodes) {
                tenantNode.NodeId = unitedStatesPoliticalPartyAffliation.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }

        }
    }
}
