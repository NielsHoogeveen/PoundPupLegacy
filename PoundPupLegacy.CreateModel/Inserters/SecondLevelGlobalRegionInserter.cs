namespace PoundPupLegacy.CreateModel.Inserters;

using Factory = SecondLevelGlobalRegionInserterFactory;
using Request = SecondLevelGlobalRegion;
using Inserter = SecondLevelGlobalRegionInserter;

public class SecondLevelGlobalRegionInserterFactory : IdentifiableDatabaseInserterFactory<Request, Inserter>
{
    internal static NonNullableIntegerDatabaseParameter FirstLevelGlobalRegionId = new() { Name = "first_level_global_region_id" };

    public override string TableName => "second_level_global_region";
}
public class SecondLevelGlobalRegionInserter : IdentifiableDatabaseInserter<Request>
{

    public SecondLevelGlobalRegionInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.FirstLevelGlobalRegionId, request.FirstLevelGlobalRegionId),
        };
    }
}
