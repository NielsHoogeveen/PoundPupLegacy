namespace PoundPupLegacy.Convert;

internal sealed class CaseTypeMigrator : MigratorPPL
{
    private readonly IMandatorySingleItemDatabaseReaderFactory<NodeIdReaderByUrlIdRequest, int> _nodeIdReaderFactory;
    private readonly IEntityCreator<CaseType> _caseTypeCreator;
    private readonly IEntityCreator<CreateNodeAction> _createNodeActionCreator;
    private readonly IEntityCreator<DeleteNodeAction> _deleteNodeActionCreator;
    private readonly IEntityCreator<EditNodeAction> _editNodeActionCreator;

    protected override string Name => "node types";

    public CaseTypeMigrator(
        IDatabaseConnections databaseConnections,
        IMandatorySingleItemDatabaseReaderFactory<NodeIdReaderByUrlIdRequest, int> nodeIdReaderFactory,
        IEntityCreator<CaseType> caseTypeCreator,
        IEntityCreator<CreateNodeAction> createNodeActionCreator,
        IEntityCreator<DeleteNodeAction> deleteNodeActionCreator,
        IEntityCreator<EditNodeAction> editNodeActionCreator
    ) : base(databaseConnections)
    {
        _nodeIdReaderFactory = nodeIdReaderFactory;
        _caseTypeCreator = caseTypeCreator;
        _createNodeActionCreator = createNodeActionCreator;
        _deleteNodeActionCreator = deleteNodeActionCreator;
        _editNodeActionCreator = editNodeActionCreator;
    }
    protected override async Task MigrateImpl()
    {
        await using var nodeIdReader = await _nodeIdReaderFactory.CreateAsync(_postgresConnection);
        await _caseTypeCreator.CreateAsync(GetCaseTypes(nodeIdReader), _postgresConnection);
        await _createNodeActionCreator.CreateAsync(GetCaseTypes(nodeIdReader).Select(x => new CreateNodeAction { Id = null, NodeTypeId = x.Id!.Value }), _postgresConnection);
        await _deleteNodeActionCreator.CreateAsync(GetCaseTypes(nodeIdReader).Select(x => new DeleteNodeAction { Id = null, NodeTypeId = x.Id!.Value }), _postgresConnection);
        await _editNodeActionCreator.CreateAsync(GetCaseTypes(nodeIdReader).Select(x => new EditNodeAction { Id = null, NodeTypeId = x.Id!.Value }), _postgresConnection);
    }
    internal async IAsyncEnumerable<CaseType> GetCaseTypes(
        IMandatorySingleItemDatabaseReader<NodeIdReaderByUrlIdRequest, int> nodeIdReader)
    {

        yield return CaseType.Create(Constants.ABUSE_CASE, "abuse case", "Abuse case of a child that has been placed by court", new List<int>
        {

            await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest
            {
                TenantId = Constants.PPL,
                UrlId = Constants.HOMESTUDY_CASE_TYPE
            }),
            await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest
            {
                TenantId = Constants.PPL,
                UrlId = Constants.PLACEMENT_CASE_TYPE
            }),
            await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest
            {
                TenantId = Constants.PPL,
                UrlId = Constants.POSTPLACEMENT_CASE_TYPE
            }),
            await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest
            {
                TenantId = Constants.PPL,
                UrlId = Constants.FACILITATION_CASE_TYPE
            }),
            await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest
            {
                TenantId = Constants.PPL,
                UrlId = Constants.INSTITUTION_CASE_TYPE
            }),
            await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest
            {
                TenantId = Constants.PPL,
                UrlId = Constants.THERAPY_CASE_TYPE
            })
        });
        yield return CaseType.Create(Constants.CHILD_TRAFFICKING_CASE, "child trafficking case", "Trafficking case of children to be adopted", new List<int>
        {
            await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest
            {
                TenantId = Constants.PPL,
                UrlId = Constants.PLACEMENT_CASE_TYPE
            }),
            await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest
            {
                TenantId = Constants.PPL,
                UrlId = Constants.FACILITATION_CASE_TYPE
            }),
            await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest
            {
                TenantId = Constants.PPL,
                UrlId = Constants.INSTITUTION_CASE_TYPE
            })
        });
        yield return CaseType.Create(Constants.COERCED_ADOPTION_CASE, "coerced adoption case", "Adoption that involved coercion", new List<int>
        {
            await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest
            {
                TenantId = Constants.PPL,
                UrlId = Constants.PLACEMENT_CASE_TYPE
            }),
        });
        yield return CaseType.Create(Constants.DEPORTATION_CASE, "deportation case", "Adoptees deported to country of origin", new List<int>());
        yield return CaseType.Create(Constants.FATHERS_RIGHTS_VIOLATION_CASE, "father's rights violation case", "Adoptions where the rights of the biological father were violated", new List<int>
        {
            await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest
            {
                TenantId = Constants.PPL,
                UrlId = Constants.PLACEMENT_CASE_TYPE
            }),
        });
        yield return CaseType.Create(Constants.WRONGFUL_MEDICATION_CASE, "wrongful medication case", "Child placement situation where wrongful medication is present", new List<int>
        {
            await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest
            {
                TenantId = Constants.PPL,
                UrlId = Constants.AUTHORITIES_CASE_TYPE
            }),
        });
        yield return CaseType.Create(Constants.WRONGFUL_REMOVAL_CASE, "wrongful removal case", "Children wrongfully removed from their family", new List<int>
        {
            await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest
            {
                TenantId = Constants.PPL,
                UrlId = Constants.AUTHORITIES_CASE_TYPE
            }),
        });
        yield return CaseType.Create(Constants.DISRUPTED_PLACEMENT_CASE, "disrupted placement case", "A situation where the placement of a child was reverted", new List<int>
        {
            await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest
            {
                TenantId = Constants.PPL,
                UrlId = Constants.PLACEMENT_CASE_TYPE
            }),
            await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest
            {
                TenantId = Constants.PPL,
                UrlId = Constants.FACILITATION_CASE_TYPE
            }),
        });

    }

}
