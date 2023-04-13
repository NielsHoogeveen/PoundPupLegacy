namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class UnitedStatesPoliticalPartyAffliationCreator : EntityCreator<UnitedStatesPoliticalPartyAffliation>
{
    private readonly IDatabaseInserterFactory<Node> _nodeInserterFactory;
    private readonly IDatabaseInserterFactory<Searchable> _searchableInserterFactory;
    private readonly IDatabaseInserterFactory<Nameable> _nameableInserterFactory;
    private readonly IDatabaseInserterFactory<UnitedStatesPoliticalPartyAffliation> _unitedStatesPoliticalPartyAffliationInserterFactory;
    private readonly IDatabaseInserterFactory<Term> _termInserterFactory;
    private readonly IMandatorySingleItemDatabaseReaderFactory<TermReaderByNameRequest, Term> _termReaderFactory;
    private readonly IDatabaseInserterFactory<TermHierarchy> _termHierarchyInserterFactory;
    private readonly IMandatorySingleItemDatabaseReaderFactory<VocabularyIdReaderByOwnerAndNameRequest, int> _vocabularyIdReaderFactory;
    private readonly IDatabaseInserterFactory<TenantNode> _tenantNodeInserterFactory;
    public UnitedStatesPoliticalPartyAffliationCreator(
        IDatabaseInserterFactory<Node> nodeInserterFactory,
        IDatabaseInserterFactory<Searchable> searchableInserterFactory,
        IDatabaseInserterFactory<Nameable> nameableInserterFactory,
        IDatabaseInserterFactory<UnitedStatesPoliticalPartyAffliation> unitedStatesPoliticalPartyAffliationInserterFactory,
        IDatabaseInserterFactory<Term> termInserterFactory,
        IMandatorySingleItemDatabaseReaderFactory<TermReaderByNameRequest, Term> termReaderFactory,
        IDatabaseInserterFactory<TermHierarchy> termHierarchyInserterFactory,
        IMandatorySingleItemDatabaseReaderFactory<VocabularyIdReaderByOwnerAndNameRequest, int> vocabularyIdReaderFactory,
        IDatabaseInserterFactory<TenantNode> tenantNodeInserterFactory
    )
    {
        _nodeInserterFactory = nodeInserterFactory;
        _searchableInserterFactory = searchableInserterFactory;
        _nameableInserterFactory = nameableInserterFactory;
        _unitedStatesPoliticalPartyAffliationInserterFactory = unitedStatesPoliticalPartyAffliationInserterFactory;
        _termInserterFactory = termInserterFactory;
        _termReaderFactory = termReaderFactory;
        _termHierarchyInserterFactory = termHierarchyInserterFactory;
        _vocabularyIdReaderFactory = vocabularyIdReaderFactory;
        _tenantNodeInserterFactory = tenantNodeInserterFactory;
    }
    public override async Task CreateAsync(IAsyncEnumerable<UnitedStatesPoliticalPartyAffliation> unitedStatesPoliticalPartyAffliations, IDbConnection connection)
    {
        await using var nodeWriter = await _nodeInserterFactory.CreateAsync(connection);
        await using var searchableWriter = await _searchableInserterFactory.CreateAsync(connection);
        await using var nameableWriter = await _nameableInserterFactory.CreateAsync(connection);
        await using var unitedStatesPoliticalPartyAffliationWriter = await _unitedStatesPoliticalPartyAffliationInserterFactory.CreateAsync(connection);
        await using var termWriter = await _termInserterFactory.CreateAsync(connection);
        await using var termReader = await _termReaderFactory.CreateAsync(connection);
        await using var termHierarchyWriter = await _termHierarchyInserterFactory.CreateAsync(connection);
        await using var vocabularyIdReader = await _vocabularyIdReaderFactory.CreateAsync(connection);
        await using var tenantNodeWriter = await _tenantNodeInserterFactory.CreateAsync(connection);

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
