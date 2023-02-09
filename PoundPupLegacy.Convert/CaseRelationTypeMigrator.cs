using PoundPupLegacy.Db;
using PoundPupLegacy.Model;

namespace PoundPupLegacy.Convert;

internal sealed class CaseRelationTypeMigrator : PPLMigrator
{
    protected override string Name => "case relation types";

    public CaseRelationTypeMigrator(MySqlToPostgresConverter converter) : base(converter)
    {

    }
    protected override async Task MigrateImpl()
    {
        await CaseRelationTypeCreator.CreateAsync(GetCaseRelationTypes(), _postgresConnection);
    }

    internal static async IAsyncEnumerable<CasePartyType> GetCaseRelationTypes()
    {
        await Task.CompletedTask;
        yield return new CasePartyType(1, "homestudy");
        yield return new CasePartyType(2, "placement");
        yield return new CasePartyType(3, "post placement");
        yield return new CasePartyType(4, "facilitation");
        yield return new CasePartyType(5, "institution");
        yield return new CasePartyType(6, "therapy");
        yield return new CasePartyType(7, "authorities");
    }

}
