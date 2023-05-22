namespace PoundPupLegacy.CreateModel.Inserters;

using Request = NewSecondLevelGlobalRegion;

public class SecondLevelGlobalRegionInserterFactory : IdentifiableDatabaseInserterFactory<Request>
{
    private static readonly NonNullableIntegerDatabaseParameter FirstLevelGlobalRegionId = new() { Name = "first_level_global_region_id" };

    public override string TableName => "second_level_global_region";
    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(FirstLevelGlobalRegionId, request.FirstLevelGlobalRegionId),
        };
    }
}
