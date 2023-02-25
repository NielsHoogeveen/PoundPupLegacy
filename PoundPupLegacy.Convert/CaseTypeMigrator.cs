using PoundPupLegacy.Db;
using PoundPupLegacy.Model;

namespace PoundPupLegacy.Convert;

internal sealed class CaseTypeMigrator : PPLMigrator
{
    protected override string Name => "node types";

    public CaseTypeMigrator(MySqlToPostgresConverter converter) : base(converter)
    {

    }
    protected override async Task MigrateImpl()
    {
        await CaseTypeCreator.CreateAsync(GetCaseTypes(), _postgresConnection);
        await CreateNodeActionCreator.CreateAsync(GetCaseTypes().Select(x => new CreateNodeAction { Id = null, NodeTypeId = x.Id!.Value }), _postgresConnection);
        await DeleteNodeActionCreator.CreateAsync(GetCaseTypes().Select(x => new DeleteNodeAction { Id = null, NodeTypeId = x.Id!.Value }), _postgresConnection);
        await EditNodeActionCreator.CreateAsync(GetCaseTypes().Select(x => new EditNodeAction { Id = null, NodeTypeId = x.Id!.Value }), _postgresConnection);

    }
    internal async IAsyncEnumerable<CaseType> GetCaseTypes()
    {
        
        yield return new CaseType(26, "abuse case", "Abuse case of a child that has been placed by court", new List<int> 
        { 

            await _nodeIdReader.ReadAsync(Constants.PPL, Constants.HOMESTUDY_CASE_TYPE),
            await _nodeIdReader.ReadAsync(Constants.PPL, Constants.PLACEMENT_CASE_TYPE),
            await _nodeIdReader.ReadAsync(Constants.PPL, Constants.POSTPLACEMENT_CASE_TYPE),
            await _nodeIdReader.ReadAsync(Constants.PPL, Constants.FACILITATION_CASE_TYPE),
            await _nodeIdReader.ReadAsync(Constants.PPL, Constants.INSTITUTION_CASE_TYPE),
            await _nodeIdReader.ReadAsync(Constants.PPL, Constants.THERAPY_CASE_TYPE)
        });
        yield return new CaseType(29, "child trafficking case", "Trafficking case of children to be adopted", new List<int> 
        {
            await _nodeIdReader.ReadAsync(Constants.PPL, Constants.PLACEMENT_CASE_TYPE),
            await _nodeIdReader.ReadAsync(Constants.PPL, Constants.FACILITATION_CASE_TYPE),
            await _nodeIdReader.ReadAsync(Constants.PPL, Constants.INSTITUTION_CASE_TYPE)
        });
        yield return new CaseType(30, "coerced adoption case", "Adoption that involved coercion", new List<int> 
        {
            await _nodeIdReader.ReadAsync(Constants.PPL, Constants.PLACEMENT_CASE_TYPE),
        });
        yield return new CaseType(31, "deportation case", "Adoptees deported to country of origin", new List<int>());
        yield return new CaseType(32, "father's rights violation case", "Adoptions where the rights of the biological father were violated", new List<int> 
        {
            await _nodeIdReader.ReadAsync(Constants.PPL, Constants.PLACEMENT_CASE_TYPE),
        });
        yield return new CaseType(33, "wrongful medication case", "Child placement situation where wrongful medication is present", new List<int> 
        {
            await _nodeIdReader.ReadAsync(Constants.PPL, Constants.AUTHORITIES_CASE_TYPE)
        });
        yield return new CaseType(34, "wrongful removal case", "Children wrongfully removed from their family", new List<int> 
        {
            await _nodeIdReader.ReadAsync(Constants.PPL, Constants.AUTHORITIES_CASE_TYPE)
        });
        yield return new CaseType(44, "disrupted placement case", "A situation where the placement of a child was reverted", new List<int> 
        {
            await _nodeIdReader.ReadAsync(Constants.PPL, Constants.PLACEMENT_CASE_TYPE),
            await _nodeIdReader.ReadAsync(Constants.PPL, Constants.FACILITATION_CASE_TYPE)
        });

    }

}
