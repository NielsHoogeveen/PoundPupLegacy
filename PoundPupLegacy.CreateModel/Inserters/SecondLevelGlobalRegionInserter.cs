namespace PoundPupLegacy.CreateModel.Inserters;

using Request = SecondLevelGlobalRegion;

public class SecondLevelGlobalRegionInserterFactory : IdentifiableDatabaseInserterFactory<Request>
{
    internal static NonNullableIntegerDatabaseParameter FirstLevelGlobalRegionId = new() { Name = "first_level_global_region_id" };

    public override string TableName => "second_level_global_region";
    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(FirstLevelGlobalRegionId, request.FirstLevelGlobalRegionId),
        };
    }
}
