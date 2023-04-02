namespace PoundPupLegacy.Convert;

internal sealed class CaseTypeMigrator : PPLMigrator
{
    protected override string Name => "node types";

    public CaseTypeMigrator(MySqlToPostgresConverter converter) : base(converter)
    {

    }
    protected override async Task MigrateImpl()
    {
        await new CaseTypeCreator().CreateAsync(GetCaseTypes(), _postgresConnection);
        await new CreateNodeActionCreator().CreateAsync(GetCaseTypes().Select(x => new CreateNodeAction { Id = null, NodeTypeId = x.Id!.Value }), _postgresConnection);
        await new DeleteNodeActionCreator().CreateAsync(GetCaseTypes().Select(x => new DeleteNodeAction { Id = null, NodeTypeId = x.Id!.Value }), _postgresConnection);
        await new EditNodeActionCreator().CreateAsync(GetCaseTypes().Select(x => new EditNodeAction { Id = null, NodeTypeId = x.Id!.Value }), _postgresConnection);

    }
    internal async IAsyncEnumerable<CaseType> GetCaseTypes()
    {

        yield return new CaseType(Constants.ABUSE_CASE, "abuse case", "Abuse case of a child that has been placed by court", new List<int>
        {

            await _nodeIdReader.ReadAsync(new NodeIdReaderByUrlId.Request
            {
                TenantId = Constants.PPL,
                UrlId = Constants.HOMESTUDY_CASE_TYPE
            }),
            await _nodeIdReader.ReadAsync(new NodeIdReaderByUrlId.Request
            {
                TenantId = Constants.PPL,
                UrlId = Constants.PLACEMENT_CASE_TYPE
            }),
            await _nodeIdReader.ReadAsync(new NodeIdReaderByUrlId.Request
            {
                TenantId = Constants.PPL,
                UrlId = Constants.POSTPLACEMENT_CASE_TYPE
            }),
            await _nodeIdReader.ReadAsync(new NodeIdReaderByUrlId.Request
            {
                TenantId = Constants.PPL,
                UrlId = Constants.FACILITATION_CASE_TYPE
            }),
            await _nodeIdReader.ReadAsync(new NodeIdReaderByUrlId.Request
            {
                TenantId = Constants.PPL,
                UrlId = Constants.INSTITUTION_CASE_TYPE
            }),
            await _nodeIdReader.ReadAsync(new NodeIdReaderByUrlId.Request
            {
                TenantId = Constants.PPL,
                UrlId = Constants.THERAPY_CASE_TYPE
            })
        });
        yield return new CaseType(Constants.CHILD_TRAFFICKING_CASE, "child trafficking case", "Trafficking case of children to be adopted", new List<int>
        {
            await _nodeIdReader.ReadAsync(new NodeIdReaderByUrlId.Request
            {
                TenantId = Constants.PPL,
                UrlId = Constants.PLACEMENT_CASE_TYPE
            }),
            await _nodeIdReader.ReadAsync(new NodeIdReaderByUrlId.Request
            {
                TenantId = Constants.PPL,
                UrlId = Constants.FACILITATION_CASE_TYPE
            }),
            await _nodeIdReader.ReadAsync(new NodeIdReaderByUrlId.Request
            {
                TenantId = Constants.PPL,
                UrlId = Constants.INSTITUTION_CASE_TYPE
            })
        });
        yield return new CaseType(Constants.COERCED_ADOPTION_CASE, "coerced adoption case", "Adoption that involved coercion", new List<int>
        {
            await _nodeIdReader.ReadAsync(new NodeIdReaderByUrlId.Request
            {
                TenantId = Constants.PPL,
                UrlId = Constants.PLACEMENT_CASE_TYPE
            }),
        });
        yield return new CaseType(Constants.DEPORTATION_CASE, "deportation case", "Adoptees deported to country of origin", new List<int>());
        yield return new CaseType(Constants.FATHERS_RIGHTS_VIOLATION_CASE, "father's rights violation case", "Adoptions where the rights of the biological father were violated", new List<int>
        {
            await _nodeIdReader.ReadAsync(new NodeIdReaderByUrlId.Request
            {
                TenantId = Constants.PPL,
                UrlId = Constants.PLACEMENT_CASE_TYPE
            }),
        });
        yield return new CaseType(Constants.WRONGFUL_MEDICATION_CASE, "wrongful medication case", "Child placement situation where wrongful medication is present", new List<int>
        {
            await _nodeIdReader.ReadAsync(new NodeIdReaderByUrlId.Request
            {
                TenantId = Constants.PPL,
                UrlId = Constants.AUTHORITIES_CASE_TYPE
            }),
        });
        yield return new CaseType(Constants.WRONGFUL_REMOVAL_CASE, "wrongful removal case", "Children wrongfully removed from their family", new List<int>
        {
            await _nodeIdReader.ReadAsync(new NodeIdReaderByUrlId.Request
            {
                TenantId = Constants.PPL,
                UrlId = Constants.AUTHORITIES_CASE_TYPE
            }),
        });
        yield return new CaseType(Constants.DISRUPTED_PLACEMENT_CASE, "disrupted placement case", "A situation where the placement of a child was reverted", new List<int>
        {
            await _nodeIdReader.ReadAsync(new NodeIdReaderByUrlId.Request
            {
                TenantId = Constants.PPL,
                UrlId = Constants.PLACEMENT_CASE_TYPE
            }),
            await _nodeIdReader.ReadAsync(new NodeIdReaderByUrlId.Request
            {
                TenantId = Constants.PPL,
                UrlId = Constants.FACILITATION_CASE_TYPE
            }),
        });

    }

}
