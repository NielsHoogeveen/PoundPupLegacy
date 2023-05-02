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

        yield return new CaseType{ 
            Id = Constants.ABUSE_CASE, 
            Name = "abuse case", 
            Description = "Abuse case of a child that has been placed by court", 
            TagLabelName = "Abuse case",
            Text = "The assumption behind child-placement is that the safety and living conditions of a child improve. These cases demonstrate that this assumption is often invalid.",
            CaseRelationTypeIds = new List<int>
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
            }
        };
        yield return new CaseType{
            Id = Constants.CHILD_TRAFFICKING_CASE, 
            Name = "child trafficking case", 
            Description = "Trafficking case of children to be adopted", 
            TagLabelName = "Child trafficking case",
            Text = "There is often a fine line between adoption and child trafficking. In many cases this line is being crossed.",
            CaseRelationTypeIds = new List<int>
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
            }
        };
        yield return new CaseType{
            Id = Constants.COERCED_ADOPTION_CASE, 
            Name = "coerced adoption case", 
            Description = "Adoption that involved coercion", 
            TagLabelName = "Coerced adoption case",
            Text = "Adoption is assumed to be the result of a choice made by the parents of the child. These cases demonstrate women are pressured to give up their children.",
            CaseRelationTypeIds = new List<int>
            {
                await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest
                {
                    TenantId = Constants.PPL,
                    UrlId = Constants.PLACEMENT_CASE_TYPE
                }),
            }
        };
        yield return new CaseType {
            Id = Constants.DEPORTATION_CASE,
            Name = "deportation case",
            Description = "Adoptees deported to country of origin",
            TagLabelName = "Deportation case",
            Text = "Adoptions before 1997, didn't automatically lead to naturalization. As result, people adopted from outside the outside US that ran into problems with the justice system face deportation to their country of birth.",
            CaseRelationTypeIds = new List<int>()
        };
        yield return new CaseType{
            Id = Constants.FATHERS_RIGHTS_VIOLATION_CASE, 
            Name = "father's rights violation case", 
            Description = "Adoptions where the rights of the biological father were violated", 
            TagLabelName = "Father's rights violation case",
            Text = "Adoption requires the consent of both biological parents. These cases demonstrate that the rights of fathers in adoption cases are being violated.",
            CaseRelationTypeIds = new List<int>
            {
                await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest
                {
                    TenantId = Constants.PPL,
                    UrlId = Constants.PLACEMENT_CASE_TYPE
                }),
            }
        };
        yield return new CaseType{
            Id = Constants.WRONGFUL_MEDICATION_CASE, 
            Name= "wrongful medication case", 
            Description = "Child placement situation where wrongful medication is present", 
            TagLabelName = "Wrongful medication case",
            Text = "Children in foster care can have serious mental health issues. Too often these children are given large doses of psychotropic medications, just to keep them quiet.",
            CaseRelationTypeIds = new List<int>
            {
                await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest
                {
                    TenantId = Constants.PPL,
                    UrlId = Constants.AUTHORITIES_CASE_TYPE
                }),
            }
        };
        yield return new CaseType{
            Id = Constants.WRONGFUL_REMOVAL_CASE, 
            Name = "wrongful removal case", 
            Description = "Children wrongfully removed from their family", 
            TagLabelName = "Wrongful removal case",
            Text = "The removal of children from their family's should always be a last resort. These cases demonstrate that Child Protective Services sometimes remove children for all the wrong reasons",
            CaseRelationTypeIds = new List<int>
            {
                await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest
                {
                    TenantId = Constants.PPL,
                    UrlId = Constants.AUTHORITIES_CASE_TYPE
                }),
            }
        };
        yield return new CaseType{
            Id = Constants.DISRUPTED_PLACEMENT_CASE, 
            Name = "disrupted placement case", 
            Description = "A situation where the placement of a child was reverted", 
            TagLabelName = "Disrupted placement case",
            Text = "Although the adoptive family is called the \"forever family\" by the adoption industry, adoptions can end in disruption. These cases demonstrate that the \"forever family\" is sometimes only temporary",
            CaseRelationTypeIds = new List<int>
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
            }
        };

    }

}
